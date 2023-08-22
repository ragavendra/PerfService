using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Grpc.Core;
using PerfRunner.V1;
using PerfRunner.Tests;
using PerfRunner.Exceptions;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;

namespace PerfRunner.Services
{
   /// <summary>
   /// Main service loading the dependencies
   /// </summary>
   public class PerfService : Perf.PerfBase
   {
      #region Fields

      private readonly ILogger<PerfService> _logger;

      private readonly ITestStateManager _testStateManager;

      private readonly IActionRunner<ITestBase> _actionRunner;

      public readonly IConfiguration _configuration;

      public readonly ITestBase _testbase;

      public readonly MeterProvider _meterProvider;

      #endregion

      public Guid Guid = Guid.NewGuid();

      public Stopwatch Stopwatch { get; set; } = new();

      public PerfService(
         ILogger<PerfService> logger,
         ITestStateManager testStateManager,
         IActionRunner<ITestBase> actionRunner,
         ITestBase testBase,
         IUserManager userManager,
         MeterProvider meterProvider,
         IConfiguration configuration)
      {
         _logger = logger;
         _testStateManager = testStateManager;
         _actionRunner = actionRunner;
         _testbase = testBase;
         _testbase.UserManager = userManager;
         _configuration = configuration;
         _meterProvider = meterProvider;
      }

      #region Methods

      public override async Task<TestReply> RunTest(TestRequest testRequest, ServerCallContext context)
      {
         _logger.LogDebug("Config - " + _configuration["SomeApp:Host"]);

         testRequest.CancellationTokenSource ??= new CancellationTokenSource();

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
         Type actionType = null;
         // List<IActionRunner<ITestBase>> actionRunners = new List<IActionRunner<ITestBase>>();

         // load actions
         foreach (var action_ in testRequest.Actions)
         {
            // check if the actions exist
            foreach (var type_ in types)
            {
               if (type_.FullName.ToLowerInvariant().EndsWith("." + action_.Name.ToLowerInvariant()))
               {
                  contains = true;
                  actionType = type_;
                  break;
               }
            }

            if (!contains)
            {
               throw new TestRequestException($"Unable to find {action_.Name} in Tests.");
            }

            contains = false;

            var inst = Activator.CreateInstance(
               actionType!,
               _testbase.HttpClient,
               _testbase.GrpcClient,
               _testbase.UserManager);

            // not going here
            if (!(inst is ITestBase typeVal))
            {
               var message = $"Does test actions {testRequest.Actions.FirstOrDefault()} exist?";
               _logger.LogError(message);
               throw new TestRequestException(message);
            }

            var actionRunner = (IActionRunner<ITestBase>)_actionRunner.CloneObj();

            // actionRunner.Guid = Guid.Parse(action_.Guid);
            _logger.LogDebug("Action guid " + action_.Guid);

            actionRunner.TypeValue = (ITestBase?)inst;

            // actionRunner.Rate = action_.Rate;
            // actionRunner.LoadDistribution_ = action_.LoadDistribution;
            // actionRunner.Paused = action_.Paused;

            /*
            if (!(action_.Duration == null))
            {
               actionRunner.Duration = action_.Duration.ToTimeSpan();
            }*/

            actionRunner.TestGuid = Guid.Parse(testRequest.Guid);

            // actionRunner.Guid = Guid.Parse(action_.Guid);
            action_.CancellationTokenSource ??= new CancellationTokenSource();
            action_.Stopwatch = new Stopwatch();

            actionRunner.ActionOption = action_;
            _logger.LogDebug("Action guid " + actionRunner.ActionOption.Guid);

            Meter meter = new Meter(_configuration["INSTR_METER"]);

            actionRunner.RunCounter = meter.CreateHistogram<double>(
               // name: actionType!.ToString() + "_" + actionRunner.Guid.ToString().Remove(6),
               name: "PerfService",
               unit: "Runs",
               description: $"No. of {actionType} run for {testRequest.Guid.Remove(6)}.");

            testRequest.ActionRunners.Add(actionRunner);
            // actionRunners.Add(actionRunner);

            // IReadOnlyList<int> list = new IReadOnlyList<int>();
            // list.Add(3);
         }

         testRequest.Stopwatch = new Stopwatch();

         if (!_testStateManager.AddTest(testRequest))
         {
            var message = $"Seems the test {testRequest.Guid} is already runing.";
            _logger.LogError(message);
            return new TestReply { Message = $"Hi {testRequest.Name} returned - {message}" };
         }

         try
         {

            testRequest.Stopwatch.Start();

            Parallel.ForEach(
               testRequest.ActionRunners,
               actionRunner =>
               {
                  // actionRunner.LoadDistribution_ = LoadDistribution.Even;
                  async void RunAct()
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

                     // keep runnung till cancelled from the client
                     while (!testRequest.CancellationTokenSource.IsCancellationRequested &&
                            !actionRunner.ActionOption.CancellationTokenSource.IsCancellationRequested)
                     {

                        var rate = 0;

                        if (actionRunner.ActionOption.Rate.Equals(0))
                        {
                           rate = testRequest.Rate;
                        }
                        else
                        {
                           rate = actionRunner.ActionOption.Rate;
                        }

                        await actionRunner.StartActionsPerSecondAsync(rate);

                        if (testRequest.CheckTestDurationElapsed())
                        {
                           _logger.LogDebug("Test {0} duration elapsed.", testRequest.Name);
                           testRequest.CancellationTokenSource.Cancel();
                        }

                        if (actionRunner.ActionOption.CheckActionDurationElapsed())
                        {
                           _logger.LogDebug("Action {0} duration elapsed.", actionRunner.ActionOption.Name);
                           actionRunner.ActionOption.CancellationTokenSource.Cancel();
                        }
                     }
                  }

                  RunAct();
               });

         }
         catch (Exception exception)
         {
            _logger.LogError("Issue running test(s) " + exception.Message);
         }

         // _logger.LogDebug(
         // "After completion, Elapsed = {0} ms",
         // (int)elapsed.TotalMilliseconds);

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
         catch (Exception ex)
         {
            _logger.LogError($"Unable to update rate - {ex.Message}");
         }


         return new UpdateRateReply { Status = true };
      }

      public override async Task MonitorTest(MonitorTestRequest monitorRequest,
      IServerStreamWriter<TestRequest> response,
      ServerCallContext context)
      {
         try
         {
            while (!context.CancellationToken.IsCancellationRequested)
            {
               var test = _testStateManager.GetTest(monitorRequest.Guid);
               await response.WriteAsync(test);
               await Task.Delay(TimeSpan.FromSeconds(3), context.CancellationToken);
            }
         }
         catch (Exception ex)
         {
            _logger.LogError($"Unable to monitor test {monitorRequest.Guid} - {ex.Message}");
         }

         // return test;
      }

      public override async Task<TestRequests> RunningTests(RunningTestsRequest runningTestsRequest, ServerCallContext context)
      {
         try
         {
            // lets update rate
            // _cancelTokenSourceAllTests.Cancel();
            var test = _testStateManager.Tests;

            // var res = (List<TestRequest>) test.Values;
            TestRequests testRequests = new TestRequests();
            testRequests.Tests.AddRange(_testStateManager.Tests.Values);

            return testRequests;
            // return new Task<TestRequests>(() => { return testRequests; });
         }
         catch (Exception ex)
         {
            _logger.LogError($"Unable to update rate - {ex.Message}");
         }

         return default;
         // return new UpdateRateReply { Status = true };
      }

      public override async Task<UpdateActionReply> UpdateAction(UpdateActionRequest updateActionRequest, ServerCallContext context)
      {
         try
         {
            var test = _testStateManager.GetTest(updateActionRequest.TestGuid);

            // var action = test.Actions.Select(action => action.Guid.Equals(updateActionRequest.Guid)).First();
            // var action = test.Actions.Where(action => action.Guid.Equals(updateActionRequest.ActionGuid)).First();

            var action = test.GetActionRunner(updateActionRequest.ActionGuid);

            _logger.LogInformation("Updating action - " + action.ActionOption.Name);

            // _actionRunner

            UpdateAction_(action);

            async void UpdateAction_(IActionRunner<ITestBase> action)
            {
               if (!action.ActionOption.Guid.Equals(updateActionRequest.ActionGuid))
               {
                  // return;
               }

               switch (updateActionRequest.ActionOptionUpdate)
               {
                  case ActionOptionUpdated.Paused:

                     action.ActionOption.Paused = bool.Parse(updateActionRequest.UpdateValue);

                     break;

                  case ActionOptionUpdated.Rate:

                     _logger.LogDebug("Updating rate from " + action.ActionOption.Rate);

                     if (int.TryParse(updateActionRequest.UpdateValue, out int res_))
                     {
                        action.ActionOption.Rate = res_;
                     }

                     _logger.LogDebug("Updated rate to " + action.ActionOption.Rate);

                     break;

                  case ActionOptionUpdated.Duration:

                     _logger.LogDebug("Updating duration from " + action.ActionOption.Duration);

                     // if no Try, Parse will cause System.FormatException and app crash*
                     if (int.TryParse(updateActionRequest.UpdateValue, out int res))
                     {
                        action.ActionOption.Stopwatch.Reset();
                        action.ActionOption.Duration = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(TimeSpan.FromSeconds(res));
                     }

                     _logger.LogDebug("Updating duration to " + action.ActionOption.Duration);

                     break;

                  case ActionOptionUpdated.Distribution:

                     _logger.LogDebug("Updating distribution from " + action.ActionOption.LoadDistribution);

                     if (System.Enum.TryParse<LoadDistribution>(updateActionRequest.UpdateValue, true, out LoadDistribution result))
                     {
                        action.ActionOption.LoadDistribution = result;
                     }

                     _logger.LogDebug("Updated distribution to " + action.ActionOption.LoadDistribution);

                     break;

                  default:

                     _logger.LogDebug("Invalid option " + updateActionRequest.ActionOptionUpdate);

                     break;
               }
            }

         }
         catch (InvalidOperationException ex)
         {
            _logger.LogError($"Unable to update action InvalidOperation - {ex.Message}");
         }
         catch (Exception ex)
         {
            _logger.LogError($"Unable to update action - {ex.Message}");
         }

         return new UpdateActionReply() { Status = true };
      }

      public override async Task<PingReply> Ping(PingRequest pingRequest, ServerCallContext context)
      {
         return new PingReply() { Message = "Hello " + pingRequest.Name };
      }

      #endregion
   }
}