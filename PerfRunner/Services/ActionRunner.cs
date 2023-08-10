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

   private bool _paused;

   private TimeSpan _duration;

   private Stopwatch _stopWatch;

   private Histogram<double> _runCounter;

   private readonly ILogger<ActionRunner<T>> _logger;

   public Guid Guid { get; set; }

   public Guid TestGuid { get; set; }

   public bool Paused { get => _paused; set => _paused = value; }

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

   public ActionBlock<T> ActionBlock { get; set; }

   public T TypeValue { get; set; }

   public TimeSpan Duration { get => _duration; set => _duration = value; }

   public ActionRunner(ILogger<ActionRunner<T>> logger)
   {
      _logger = logger;
      _stopWatch = new Stopwatch();
   }

   // Initiates several computations by using dataflow and returns the elapsed
   // time required to initiate the computations.
   public async Task<bool> StartActionsPerSecondAsync(int rate)
   {
      // loop if paused
      while (_paused)
      {
         Thread.Sleep(300);
      }

      _stopWatch.Start();

      if((Duration != null) && (Duration!.TotalSeconds > 0))
      {
         if (_stopWatch.Elapsed.TotalSeconds > Duration.TotalSeconds)
         {
            _logger?.LogDebug(
               $"After duration, Elapsed = {_stopWatch.Elapsed.TotalMilliseconds} s for {Guid}");
            return true;
         }
      }

      // Compute the time that it takes for several messages to
      // flow through the dataflow block.
      var sw = new Stopwatch();
      sw.Start();

      var divisor = 0;
      var remaining = 0;

      // get rate by 1000 ms to post in intervals
      var divisor_ = 1000 / rate;

      int indexer = 0;

      while (rate-- > 0)
      {
         // remaining = divisor;
         if (_loadDistribution?.Equals(LoadDistribution.Uneven) == true)
         {
            var rand = new Random();
            divisor = rand.Next(divisor_);

            // _logger?.LogInformation($"Divisor - {divisor} .");
            remaining = divisor_ - divisor;

            // Thread.Sleep(divisor_);
         }
         else
         {
            divisor = divisor_;
         }

         Thread.Sleep(divisor);
         // divisor = divisor_;

         ActionBlock.Post(TypeValue);
         _logger?.LogDebug(
            $"After Posting, elapsed - {sw.Elapsed.TotalMilliseconds} ms, waited for remain - {remaining} ms. and divisor - {divisor} ms"
         );

         // update instr
         UpdateInstr(sw.Elapsed.TotalMilliseconds, indexer++);

         Thread.Sleep(remaining);
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

      return false;
   }

   public async void UpdateInstr(double totalMilliseconds, int interation)
   {
         TagList taglist = new TagList();
         taglist.Add("action", TypeValue!.GetType());
         taglist.Add("iteration", interation);
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