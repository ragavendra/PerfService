namespace PerfRunner.Services;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Dataflow;

/* Sample output:
Processor count = 4.
Degree of parallelism = 1; message count = 4; elapsed time = 4032ms.
Degree of parallelism = 4; message count = 4; elapsed time = 1001ms.
*/
/*
Test runner should take the test (List of action(s)), rate per second
and start the test first.
*/

// Demonstrates how to specify the maximum degree of parallelism
// when using dataflow.
public class ActionRunner<T>
{

   public Guid Guid = Guid.NewGuid();

   public ActionBlock<T> ActionBlock { get; set; }

   public IList<ActionBlock<T>> ActionBlocks { get; set; } = new List<ActionBlock<T>>();

   public T TypeValue { get; set; }

   public IList<T> TypeValues { get; set; }

   public Stopwatch Stopwatch { get; set; } = new();

   private readonly ILogger<ActionRunner<T>> _logger;

   public ActionRunner(ILogger<ActionRunner<T>> logger)
   {
      _logger = logger;
   }

   // Initiates several computations by using dataflow and returns the elapsed
   // time required to initiate the computations.
   public async Task<TimeSpan> StartActionsPerSecondAsync(int rate)
   {
      // Compute the time that it takes for several messages to
      // flow through the dataflow block.
      var sw = new Stopwatch();
      sw.Start();

      while (rate-- > 0)
      {
         ActionBlock.Post(TypeValue);
      }

      // no more to post 
      ActionBlock.Complete();

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      while (sw.Elapsed.TotalMilliseconds <= 1000)
      {
         Thread.Sleep(100);
      }

      sw.Stop();

      _logger?.LogInformation(
         $"After complete, Elapsed = {sw.Elapsed.TotalMilliseconds} ms for {Guid}");

      return sw.Elapsed;
   }

   public object CloneObj()
   {
      return MemberwiseClone();
   }
}