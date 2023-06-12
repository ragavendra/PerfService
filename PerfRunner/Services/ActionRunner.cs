namespace PerfRunner.Services;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using PerfRunner.V1;

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
public class ActionRunner<T> : IActionRunner<T>
{
   private LoadDistribution? _loadDistribution;

   public LoadDistribution? LoadDistribution_ { get { return _loadDistribution; } set { _loadDistribution = value; } }

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

      var divisor = 0;

      // get rate by 1000 ms to post in intervals
      divisor = 1000 / rate;

      while (rate-- > 0)
      {
         ActionBlock.Post(TypeValue);

         if (_loadDistribution?.Equals(LoadDistribution.Uneven) == true)
         {
            var rand = new Random();
            divisor = rand.Next(divisor);
         }

         Thread.Sleep(divisor);
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