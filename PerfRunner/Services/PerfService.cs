using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Grpc.Core;
using PerfRunner.V1;
using PerfRunner.Tests;
using System.Reflection;

namespace PerfRunner.Services
{
   public class PerfService : Perf.PerfBase
   {
      private readonly ILogger<PerfService> _logger;

      private readonly ITestStateManager _testStateManager;

      private readonly IActionRunner<ITestBase> _actionRunner;

      public readonly IConfiguration _configuration;

      public readonly ITestBase _testbase;

      public Guid Guid = Guid.NewGuid();

      public Stopwatch Stopwatch { get; set; } = new();

      public PerfService(
         ILogger<PerfService> logger,
         ITestStateManager testStateManager,
         IActionRunner<ITestBase> actionRunner,
         ITestBase testBase,
         IUserManager userManager,
         IConfiguration configuration)
      {
         _logger = logger;
         _testStateManager = testStateManager;
         _actionRunner = actionRunner;
         _testbase = testBase;
         _testbase.UserManager = userManager;
         _configuration = configuration;
      }
 
      public override async Task<TestReply> RunTest(TestRequest testRequest, ServerCallContext context)
      {
         _logger.LogDebug("Config - " + _configuration["SomeApp:Host"]);

         testRequest.CancellationTokenSource = new CancellationTokenSource();

         // depending on the no of processors
         // runs so many in parallel
         int processorCount = Environment.ProcessorCount;

         // Print the number of processors on this computer.
         _logger?.LogTrace("Processor count = {0}.", processorCount);

         TimeSpan elapsed = TimeSpan.MinValue;

         //lets get all the types of ITestBase
         var type = typeof(ITestBase);
         var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
         // types.First().

         bool contains = false;

         // load actions
         foreach (var action_ in testRequest.Actions)
         {
            // check if the actions exist
            foreach (var type_ in types)
            {
               if(type_.FullName.ToLowerInvariant().EndsWith("." + action_.Name.ToLowerInvariant()))
               {
                  contains = true;
               } 
            }

            if(!contains)
            {
               throw new TestRequestException($"Unable to find {action_.Name} in Tests.");
            }

            contains = false;

            var inst = Activator.CreateInstance(
               types.First(action => action.FullName.ToLowerInvariant()
                  .EndsWith("." + action_.Name.ToLowerInvariant())),
               _testbase.HttpClient,
               _testbase.GrpcClient,
               _testbase.UserManager);

            // throw new TestRequestException("test message");

            // not going here
            if (!(inst is ITestBase typeVal))
            {
               var message = $"Does test actions {testRequest.Actions.FirstOrDefault()} exist?";
               _logger.LogError(message);
               throw new TestRequestException(message);
            }

            var actionRunner = (IActionRunner<ITestBase>)_actionRunner.CloneObj();

            actionRunner.TypeValue = (ITestBase?)inst;

            actionRunner.Rate = action_.Rate;

            testRequest.ActionRunners.Add(actionRunner);
         }

         if (!_testStateManager.AddTest(testRequest))
         {
            var message = $"Seems the test {testRequest.Guid} is already runing.";
            _logger.LogError(message);
            return new TestReply { Message = $"Hi {testRequest.Name} returned - {message}" };
         }

         Parallel.ForEach(
            testRequest.ActionRunners,
            actionRunner =>
            {
               actionRunner.LoadDistribution_ = LoadDistribution.Even;
               async void RunAct()
               {
                  // keep runnung till cancelled from the client
                  while (!testRequest.CancellationTokenSource.IsCancellationRequested)
                  {

                     // Create an ActionBlock<int> that performs some work.
                     actionRunner.ActionBlock = new ActionBlock<ITestBase>(

                        // Simulate work by suspending the current thread.
                        testBase => testBase.RunTest(Guid, _logger),

                        // Specify a maximum degree of parallelism.
                        new ExecutionDataflowBlockOptions
                        {
                           MaxDegreeOfParallelism = processorCount
                        }
                           );

                     var rate = 0;

                     if (actionRunner.Rate.Equals(0))
                     {
                        rate = testRequest.Rate;
                     }
                     else
                     {
                        rate = actionRunner.Rate;
                     }

                     elapsed = await actionRunner.StartActionsPerSecondAsync(rate);
                  }
               }

               RunAct();
            });

         _logger.LogDebug(
            "After completion, Elapsed = {0} ms",
            (int)elapsed.TotalMilliseconds);

         return new TestReply { Message = $"Hi {testRequest.Name}" };
      }

      public override async Task<StopTestReply> StopTest(StopTestRequest stopTestRequest, ServerCallContext context)
      {
         bool resp = false;
         try
         {
            var test = _testStateManager.GetTest(stopTestRequest.Guid);
            test?.CancellationTokenSource.Cancel();
            resp = _testStateManager.RemoveTest(test.Guid);
         }
         catch (InvalidOperationException ex)
         {
            var message = $"Issue stopping test ex - {ex.Message} .";
            _logger.LogError(message);
            return new StopTestReply { Status = resp };
         }
         catch (Exception ex)
         {
            var message = $"Issue stopping test ex - {ex.Message} .";
            _logger.LogError(message);
            return new StopTestReply { Status = resp };
         }

         return new StopTestReply { Status = resp };
      }

      public override async Task<StopAllTestsReply> StopAllTests(StopAllTestsRequest stopAllTestsRequest, ServerCallContext context)
      {
         foreach (var test in _testStateManager.Tests)
         {
            test.Value.CancellationTokenSource.Cancel();
         }

         return new StopAllTestsReply { Status = true };
      }

      public override async Task<UpdateRateReply> UpdateRate(UpdateRateRequest updateRateRequest, ServerCallContext context)
      {
         try
         {
            // lets update rate
            // _cancelTokenSourceAllTests.Cancel();
            var test = _testStateManager.GetTest(updateRateRequest.Guid);
            test.Rate = updateRateRequest.Rate;

            /*
            // increase or decrease rate and since same action block add or - the same item
            while (!test.ActionRunner.ActionBlocks.Count.Equals(updateRateRequest.Rate))
            {
               if (test.ActionRunner.ActionBlocks.Count > updateRateRequest.Rate)
               {
                  test.ActionRunner.ActionBlocks.RemoveAt(0);
               }
               else
               {
                  test.ActionRunner.ActionBlocks.Add(test.ActionRunner.ActionBlocks.First());
               }
            }*/
         }
         catch(Exception ex)
         {
            _logger.LogError($"Unable to update rate - {ex.Message}");
         }


         return new UpdateRateReply { Status = true };
      }
   }
}