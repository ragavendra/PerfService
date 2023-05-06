using System.Threading.Tasks.Dataflow;
using Grpc.Core;
using PerfRunner.Network;

namespace PerfRunner.Services
{
   public class PerfService : Perf.PerfBase
   {
      private readonly ILogger<PerfService> _logger;
      private readonly IHttp _http;
      private readonly IGrpc _grpc;

      private static CancellationTokenSource _cancelTokenSourceAllTests;

      public PerfService(ILogger<PerfService> logger, IHttp http, IGrpc grpc)
      {
         _logger = logger;
         _http = http;
         _grpc = grpc;
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

        _cancelTokenSourceAllTests = new CancellationTokenSource();
        
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
         while(!_cancelTokenSourceAllTests.IsCancellationRequested)
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

         /*
                  return Task.FromResult(new TestReply
                  {
                     Message = $"Hi {testRequest.Name}"
                  });*/
      }

      public override async Task<StopAllTestsReply> StopAllTests(StopAllTestsRequest stopAllTestsRequest, ServerCallContext context)
      {
        // lets try cancel
        _cancelTokenSourceAllTests.Cancel();

         return new StopAllTestsReply { Status = true };
      }
   }
}