namespace PerfRunner.Services;

using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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

/// <summary>
/// One ActionRunner<T> for each action of type <T> or test to be run.
/// </summary>
public class ActionRunner<T> : IActionRunner<T>
{
   private LoadDistribution? _loadDistribution;

   private int _rate;

   public Guid Guid { get; } = Guid.NewGuid();

   public Guid TestGuid { get; set; }

   public LoadDistribution? LoadDistribution_ { get { return _loadDistribution; } set { _loadDistribution = value; } }

   public int Rate
   {
      get { return _rate; }
      set
      {
         if (value > 0)
         {
            _rate = value;
         }
      }
   }

   public Histogram<double> RunCounter { get => _runCounter; set => _runCounter = value; }

   private Histogram<double> _runCounter;

   public ActionBlock<T> ActionBlock { get; set; }

   public T TypeValue { get; set; }

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
      var remaining = 0;

      // get rate by 1000 ms to post in intervals
      var divisor_ = 1000 / rate;

      while (rate-- > 0)
      {
         ActionBlock.Post(TypeValue);
         _logger?.LogDebug(
            $"After Posting, elapsed - {sw.Elapsed.TotalMilliseconds} ms, waited for remain - {remaining} ms. and divisor - {divisor} ms"
         );

         // update instr
         UpdateInstr(sw.Elapsed.TotalMilliseconds);

         Thread.Sleep(remaining);

         // remaining = divisor;
         if (_loadDistribution?.Equals(LoadDistribution.Uneven) == true)
         {
            var rand = new Random();
            divisor = rand.Next(divisor_);

            // _logger?.LogInformation($"Divisor - {divisor} .");
            remaining = divisor_ - divisor;
         }
         else
         {
            divisor = divisor_;
         }

         Thread.Sleep(divisor);
         // divisor = divisor_;
      }

      // no more to post 
      // ActionBlock.Complete();

      // Wait for all messages to propagate through the network.
      // workerBlock.Completion.Wait();

      while (sw.Elapsed.TotalMilliseconds <= 1000)
      {
         Thread.Sleep(100);
      }

      sw.Stop();

      _logger?.LogDebug(
         $"After complete, Elapsed = {sw.Elapsed.TotalMilliseconds} ms for {Guid}");

      return sw.Elapsed;
   }

   public async void UpdateInstr(double totalMilliseconds)
   {
         TagList taglist = new TagList();
         taglist.Add("action", TypeValue!.GetType());
         taglist.Add("guid", TestGuid.ToString().Remove(6));
         // taglist.Add("action-guid", Guid.ToString().Remove(6));

         // record time takes in ms for each call
         _runCounter.Record(totalMilliseconds, taglist);
   }

   public object CloneObj()
   {
      return MemberwiseClone();
   }
}