namespace PerfRunner;

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
internal class ActionRunner<T>
{
   public IList<ActionBlock<T>> ActionBlocks { get; set; } = new List<ActionBlock<T>>();

   public T TypeValue { get; set; }

   public Stopwatch Stopwatch { get; set; } = new();

   private readonly ILogger<ActionRunner<T>> _logger;

   public ActionRunner(ILogger<ActionRunner<T>> logger)
   {
      _logger = logger;
   }

   public ActionRunner()
   {
      // _logger = new Logger<ActionRunner<T>>();
   }

   // Initiates several computations by using dataflow and returns the elapsed
   // time required to initiate the computations.
   public async Task <TimeSpan> StartActionsPerSecondAsync()
   {
      // Compute the time that it takes for several messages to
      // flow through the dataflow block.
      // Stopwatch stopwatch = new();

      Stopwatch.Start();

      var result = ActionBlocks.Select(action => action.Post(TypeValue));

      // no more to post 
      // ActionBlock.Complete();
      // result = ActionBlocks.Select(action => action.Complete());
      // ActionBlocks.ForEach(action => action.Complete());
      foreach (var item in ActionBlocks)
      {
         item.Complete();
         item.Completion.Wait();
      }

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      while (Stopwatch.Elapsed.TotalMilliseconds <= 1000) { 
         Thread.Sleep(100);
      }

      // Stop the timer and return the elapsed number of milliseconds.
      // stopwatch.Stop();

      return Stopwatch.Elapsed;
   }
}