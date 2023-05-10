using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.Linq.Expressions;
using System;
using Grpc.Core;
using PerfRunner.Network;
using PerfRunner.V1;
using PerfRunner.Tests;

namespace PerfRunner.Services
{
   public class PerfService : Perf.PerfBase
   {
      private readonly ILogger<PerfService> _logger;

      private readonly IHttp _http;

      private readonly IGrpc _grpc;

      private readonly TestStateManager _testStateManager;

      private readonly ActionRunner<ITestBase> _actionRunner;

      public Guid Guid = Guid.NewGuid();

      public Stopwatch Stopwatch { get; set; } = new();

      public PerfService(
         ILogger<PerfService> logger,
         IHttp http,
         IGrpc grpc,
         TestStateManager testStateManager,
         ActionRunner<ITestBase> actionRunner)
      {
         _logger = logger;
         _http = http;
         _grpc = grpc;
         _testStateManager = testStateManager;
         _actionRunner = actionRunner;
      }

      private void SomeFunc(int millisecondsTimeout)
      {
         Thread.Sleep(millisecondsTimeout);
         _logger.LogInformation("NowI in SomeFunc - " + Guid);
         // Console.WriteLine("Now in SomeFunc - " + Guid);
      }

      public override async Task<TestReply> RunTest(TestRequest testRequest, ServerCallContext context)
      {
         _logger.LogInformation("Message from Http service - " + _http.SampleHttpMethod());
         _logger.LogInformation("Message from Grpc service - " + _grpc.SampleGrpcMethod());

        testRequest.CancellationTokenSource = new CancellationTokenSource();

         // depending on the no of processors
         // runs so many in parallel
         int processorCount = Environment.ProcessorCount;
         const int howMany = 12;

         // Print the number of processors on this computer.
         Console.WriteLine("Processor count = {0}.", processorCount);

         TimeSpan elapsed = TimeSpan.MinValue;

         // Perform two dataflow computations and print the elapsed
         // time required for each.
         // testRequest.ActionRunner = new ActionRunner<int>((ILogger<ActionRunner<int>>)_logger){ TypeValue = 1000 };
         // testRequest.ActionRunner = new ActionRunner<int>(){ TypeValue = 10 };
         // _actionRunner.TypeValue = 10;
         // testRequest.ActionRunner = new ActionRunner<TestBase>();
         _actionRunner.TypeValue = new Login();

         // "SomeStr"
         testRequest.ActionRunner = _actionRunner;

         if(!_testStateManager.AddTest(testRequest))
         {
           _logger.LogError($"Seems the test {testRequest.Guid} is already runing"); 
           return default;
         }

         // keep runnung till cancelled from the client
         while (!testRequest.CancellationTokenSource.IsCancellationRequested)
         {

            // Create an ActionBlock<int> that performs some work.
            testRequest.ActionRunner.ActionBlock = new ActionBlock<ITestBase>(

               // Simulate work by suspending the current thread.
               testBase => testBase.RunTest(Guid),

               // Specify a maximum degree of parallelism.
               new ExecutionDataflowBlockOptions
               {
                  MaxDegreeOfParallelism = processorCount
               }
                  );

            elapsed = await testRequest.ActionRunner.StartActionsPerSecondAsync(testRequest.Rate);
         }

         // actionRunner.ActionBlocks.Select(item => item.Completion.Wait());
         /*
         foreach (var item in testRequest.ActionRunner.ActionBlocks)
         {
            item.Completion.Wait();
         }*/
         // testRequest.ActionRunner.ActionBlock.Completion.Wait();

         // actionRunner.ActionBlock.Completion.Wait();
         // testRequest.ActionRunner.Stopwatch.Stop();

         _logger.LogInformation(
            "After completion, Elapsed = {0} ms",
            (int)elapsed.TotalMilliseconds);

         var reply = new TestReply { Message = $"Hi {testRequest.Name}" };

         return reply;
      }

      public override async Task<StopTestReply> StopTest(StopTestRequest stopTestRequest, ServerCallContext context)
      {
        var test = _testStateManager.GetTest(stopTestRequest.Guid);
        test?.CancellationTokenSource.Cancel();
        var resp = _testStateManager.RemoveTest(test.Guid);

         return new StopTestReply { Status = resp };
      }

      public override async Task<StopAllTestsReply> StopAllTests(StopAllTestsRequest stopAllTestsRequest, ServerCallContext context)
      {
         foreach (var test in _testStateManager.Tests)
         {
            test.Value.CancellationTokenSource.Cancel();
         }

        // lets try cancel
        // _cancelTokenSourceAllTests.Cancel();

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