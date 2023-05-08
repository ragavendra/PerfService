using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Grpc.Core;
using PerfRunner.Network;
using PerfRunner.V1;

namespace PerfRunner.Services
{
   public class PerfService : Perf.PerfBase
   {
      private readonly ILogger<PerfService> _logger;

      private readonly IHttp _http;

      private readonly IGrpc _grpc;

      private readonly TestStateManager _testStateManager;

      public Guid Guid = Guid.NewGuid();

      public Stopwatch Stopwatch { get; set; } = new();

      public PerfService(ILogger<PerfService> logger, IHttp http, IGrpc grpc, TestStateManager testStateManager)
      {
         _logger = logger;
         _http = http;
         _grpc = grpc;
         _testStateManager = testStateManager;
      }

      private void SomeFunc(int millisecondsTimeout)
      {
         Thread.Sleep(millisecondsTimeout);
         _logger.LogInformation("Now in SomeFunc");
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

         TimeSpan elapsed;

         // Perform two dataflow computations and print the elapsed
         // time required for each.
         testRequest.ActionRunner = new ActionRunner<int>(){ TypeValue = 1000 };

         if(!_testStateManager.AddTest(testRequest))
         {
           _logger.LogError($"Seems the test {testRequest.Guid} is already runing"); 
           return default;
         }

         // keep runnung till cancelled from the client
         while (!testRequest.CancellationTokenSource.IsCancellationRequested)
         {

            // Create an ActionBlock<int> that performs some work.
            var actionBlock = new ActionBlock<int>(

               // Simulate work by suspending the current thread.
               millisecondsTimeout => SomeFunc(millisecondsTimeout),

               // Specify a maximum degree of parallelism.
               new ExecutionDataflowBlockOptions
               {
                  MaxDegreeOfParallelism = processorCount
               }
                  );

            for (int iIndex = 0; iIndex < testRequest.Rate; iIndex++)
            {
               testRequest.ActionRunner.ActionBlocks.Add(actionBlock);
            }

            elapsed = await testRequest.ActionRunner.StartActionsPerSecondAsync();

            testRequest.ActionRunner.ActionBlocks.Clear();

            /*
            Console.WriteLine(
               "Degree of parallelism = {0}; message count = {1}; " +
                  "elapsed time = {2}ms.",
               processorCount,
               actionRunner.ActionBlocks.Count,
               (int)elapsed.TotalMilliseconds);*/

         }

         // actionRunner.ActionBlocks.Select(item => item.Completion.Wait());
         foreach (var item in testRequest.ActionRunner.ActionBlocks)
         {
            item.Completion.Wait();
         }

         // actionRunner.ActionBlock.Completion.Wait();
         testRequest.ActionRunner.Stopwatch.Stop();

         _logger.LogInformation(
            "After completion, Elapsed = {0} ms",
            (int)testRequest.ActionRunner.Stopwatch.Elapsed.TotalMilliseconds);

         var reply = new TestReply { Message = $"Hi {testRequest.Name}" };

         return reply;
      }

      public override async Task<StopTestReply> StopTest(StopTestRequest stopTestRequest, ServerCallContext context)
      {
        _testStateManager.GetTest(stopTestRequest.Guid)?.CancellationTokenSource.Cancel();

         return new StopTestReply { Status = true };
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
         // lets update rate
         // _cancelTokenSourceAllTests.Cancel();
         var test = _testStateManager.GetTest(updateRateRequest.Guid);
         test.Rate = updateRateRequest.Rate;

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
         }

         return new UpdateRateReply { Status = true };
      }
   }
}