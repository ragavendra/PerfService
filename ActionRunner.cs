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

// Demonstrates how to specify the maximum degree of parallelism
// when using dataflow.
internal class ActionRunner<T>
{
   public ActionBlock<T> ActionBlock { get; set; }

   // Initiates several computations by using dataflow and returns the elapsed
   // time required to initiate the computations.
   public async Task <TimeSpan> StartActionsAsync(
      T typeValue,
      int messageCount)
   {
      // Compute the time that it takes for several messages to
      // flow through the dataflow block.
      Stopwatch stopwatch = new();

      stopwatch.Start();

      for (int i = 0; i < messageCount; i++)
      {
         ActionBlock.Post(typeValue);
      }

      ActionBlock.Complete();

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      // while (stopwatch.Elapsed.TotalMilliseconds <= 1000) { }

      // Stop the timer and return the elapsed number of milliseconds.
      stopwatch.Stop();

      return stopwatch.Elapsed;
   }
}

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Program
{
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string DebuggerDisplay => ToString();
   
   private static void SomeFunc(int millisecondsTimeout)
   {
      Thread.Sleep(millisecondsTimeout);
      Console.WriteLine("Now in SomeFunc");
   }

   static async Task Main(string[] args)
   {
      int processorCount = Environment.ProcessorCount;
      const int howMany = 12;

      // Print the number of processors on this computer.
      Console.WriteLine("Processor count = {0}.", processorCount);

      TimeSpan elapsed;

      // Perform two dataflow computations and print the elapsed
      // time required for each.
      var actionRunner = new ActionRunner<int>();

      const int count = 1000;

      const int noThreads = 10;
      // Create an ActionBlock<int> that performs some work.
      actionRunner.ActionBlock = new ActionBlock<int>(

          // Simulate work by suspending the current thread.
         millisecondsTimeout => SomeFunc(millisecondsTimeout),

          // Specify a maximum degree of parallelism.
         new ExecutionDataflowBlockOptions
            {
               MaxDegreeOfParallelism = noThreads
            }
            );

      elapsed = await actionRunner.StartActionsAsync(count, howMany);
      Console.WriteLine(
         "Degree of parallelism = {0}; message count = {1}; " +
            "elapsed time = {2}ms.",
         noThreads,
         howMany,
         (int)elapsed.TotalMilliseconds);

      actionRunner.ActionBlock.Completion.Wait();
   }
}