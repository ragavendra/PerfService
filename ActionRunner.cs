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

   // Initiates several computations by using dataflow and returns the elapsed
   // time required to initiate the computations.
   public async Task <TimeSpan> StartActionsAsync()
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
      }

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      // while (stopwatch.Elapsed.TotalMilliseconds <= 1000) { }

      // Stop the timer and return the elapsed number of milliseconds.
      // stopwatch.Stop();

      return Stopwatch.Elapsed;
   }
}

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Program_
{
   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
   private string DebuggerDisplay => ToString();
   
   private void SomeFunc(int millisecondsTimeout)
   {
      Thread.Sleep(millisecondsTimeout);
      Console.WriteLine("Now in SomeFunc");
   }

   async Task Main_(string[] args)
   {
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

      actionRunner.ActionBlocks.Add(actionBlock);
      actionRunner.ActionBlocks.Add(actionBlock);
      actionRunner.ActionBlocks.Add(actionBlock);

      elapsed = await actionRunner.StartActionsAsync();
      Console.WriteLine(
         "Degree of parallelism = {0}; message count = {1}; " +
            "elapsed time = {2}ms.",
         processorCount,
         actionRunner.ActionBlocks.Count,
         (int)elapsed.TotalMilliseconds);

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
   }
}