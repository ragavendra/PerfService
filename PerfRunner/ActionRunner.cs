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

   public ActionBlock<T> ActionBlock { get; set; }

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
   public async Task <TimeSpan> StartActionsPerSecondAsync(int rate)
   {
      // Compute the time that it takes for several messages to
      // flow through the dataflow block.
      // Stopwatch stopwatch = new();

      var sw = new Stopwatch();
      sw.Start();

      // var result = ActionBlocks.Select(action => action.Post(TypeValue));
      while(rate-- > 0)
      {
         ActionBlock.Post(TypeValue);
      }

      // no more to post 
      // ActionBlock.Complete();
      // result = ActionBlocks.Select(action => action.Complete());
      // ActionBlocks.ForEach(action => action.Complete());
      /*
      foreach (var item in ActionBlocks)
      {
         item.Complete();
         item.Completion.Wait();
      }*/
      ActionBlock.Complete();

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      while (sw.Elapsed.TotalMilliseconds <= 1000) { 
         Thread.Sleep(100);
      }

      // ActionBlock.Completion.Wait();

      // Stop the timer and return the elapsed number of milliseconds.
      // stopwatch.Stop();
      sw.Stop();
      Console.WriteLine(
         "After complete, Elapsed = {0} ms",
         (int)sw.Elapsed.TotalMilliseconds);

      return sw.Elapsed;
   }
}