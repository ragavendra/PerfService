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

      private TestRequest _testRequest;

      private readonly TestStateManager _testStateManager;

      private readonly CancellationTokenSource _cancelTokenSource;

      public Guid Guid = Guid.NewGuid();

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

         if(!_testStateManager.AddTest(testRequest))
         {
           _logger.LogError($"Seems the test {testRequest.Guid} is already runing"); 
           return default;
         }

         _testRequest = testRequest;

        // _cancelTokenSourceAllTests = new CancellationTokenSource();
        // testRequest.CancellationTokenSource = new CancellationTokenSource();
        
        // context.CancellationToken = cancellationTokenSource.Token;
        // _ = ThreadPool.QueueUserWorkItem(new WaitCallback(SomeFunc), cancellationTokenSource);

         // depending on the no of processors
         // runs so many in parallel
         int processorCount = Environment.ProcessorCount;
         const int howMany = 12;

         // Print the number of processors on this computer.
         Console.WriteLine("Processor count = {0}.", processorCount);

         TimeSpan elapsed;

         // Perform two dataflow computations and print the elapsed
         // time required for each.
         var actionRunner = new ActionRunner<int>();

         /*
               List<string> list = new List<string>(){ "one", "six", "seven" };
               var item = list.Where(item => item.Equals("one"));*/

         // how many in milli seconds to wait
         const int count = 1000;

         const int noThreads = 10;

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
            actionRunner.ActionBlocks.Add(actionBlock);
         }

         // keep runnung till cancelled from the client
         while(!testRequest.CancellationTokenSource.IsCancellationRequested)
         {
            elapsed = await actionRunner.StartActionsPerSecondAsync();

            /*
            Console.WriteLine(
               "Degree of parallelism = {0}; message count = {1}; " +
                  "elapsed time = {2}ms.",
               processorCount,
               actionRunner.ActionBlocks.Count,
               (int)elapsed.TotalMilliseconds);*/

         }

         // actionRunner.ActionBlocks.Select(item => item.Completion.Wait());
         foreach (var item in actionRunner.ActionBlocks)
         {
            item.Completion.Wait();
         }

         // actionRunner.ActionBlock.Completion.Wait();
         actionRunner.Stopwatch.Stop();

         Console.WriteLine(
            "After completion, Elapsed = {0} ms",
            (int)actionRunner.Stopwatch.Elapsed.TotalMilliseconds);

         var reply = new TestReply { Message = $"Hi {testRequest.Name}" };

         return reply;
      }

      public override async Task<StopTestReply> StopTest(StopTestRequest stopTestRequest, ServerCallContext context)
      {
        // _testStateManager.Tests.Where(test => test.Key.ToString().
        // Equals(stopTestRequest.Guid)).First().Value.CancellationTokenSource.Cancel();
        _testRequest.CancellationTokenSource.Cancel();

         return new StopTestReply { Status = true };
      }

      public override async Task<StopAllTestsReply> StopAllTests(StopAllTestsRequest stopAllTestsRequest, ServerCallContext context)
      {
        // lets fetch all tests from test state manager
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

         return new UpdateRateReply { Status = true };
      }
   }
}