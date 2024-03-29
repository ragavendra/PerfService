﻿using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks.Dataflow;
using PerfRunner.V1;

namespace PerfRunner.Services;

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
/// <typeparamref name="T" />
/// </summary>
public class ActionRunner<T> : IActionRunner<T>
{
    private Histogram<double>? _runHistogram;

    private Counter<int>? _runCounter;

    private ActionOption? _actionOption;

    private readonly ILogger<ActionRunner<T>> _logger;

    public Guid Guid { get; set; }

    public Guid TestGuid { get; set; }

    public ActionOption? ActionOption { get => _actionOption; set => _actionOption = value; }

    public Histogram<double>? RunHistogram { get => _runHistogram; set => _runHistogram = value; }

    public Counter<int>? RunCounter { get => _runCounter; set => _runCounter = value; }

    public ActionBlock<T>? ActionBlock { get; set; }

    public T? TypeValue { get; set; }

    public ActionRunner(ILogger<ActionRunner<T>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Initiates several computations by using dataflow and returns the elapsed
    /// time required to initiate the computations.
    /// </summary>
    public async Task<bool> StartActionsPerSecondAsync(int rate)
    {
        // loop if paused
        while (ActionOption.Paused)
        {
            Thread.Sleep(300);
        }

        ActionOption.Stopwatch.Start();

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
            if (_actionOption.LoadDistribution.Equals(LoadDistribution.Uneven) == true)
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

            ActionBlock?.Post(TypeValue!);
            _logger?.LogDebug(
                    $"After Posting, elapsed - {sw.Elapsed.TotalMilliseconds.ToString()} ms, waited for remain - {remaining.ToString()} ms. and divisor - {divisor.ToString()} ms"
                    );

            // update  hist
            UpdateHistAsync(sw.Elapsed.TotalMilliseconds, indexer++);

            Thread.Sleep(remaining);
        }

        // update  hist
        UpdateCntrAsync(sw.Elapsed.TotalMilliseconds, indexer);

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
                $"After complete, Elapsed = {sw.Elapsed.TotalMilliseconds.ToString()} ms for {Guid.ToString()}");

        return false;
    }

    public async void UpdateCntrAsync(double totalMilliseconds, int iteration)
    {
        TagList taglist = new TagList();
        taglist.Add("action", TypeValue!.GetType());
        // taglist.Add("iteration", iteration);
        // taglist.Add("period", totalMilliseconds.ToString());
        taglist.Add("guid", TestGuid.ToString().Remove(6));
        taglist.Add("host", Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));

        // record counter
        _runCounter?.Add(iteration, taglist);
    }

    public async void UpdateHistAsync(double totalMilliseconds, int iteration)
    {
        TagList taglist = new TagList();
        taglist.Add("action", TypeValue!.GetType());
        taglist.Add("iteration", iteration);
        taglist.Add("guid", TestGuid.ToString().Remove(6));
        taglist.Add("host", Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
        // taglist.Add("action-guid", Guid.ToString().Remove(6));

        // record time takes in ms for each call
        _runHistogram?.Record(totalMilliseconds, taglist);
    }

    public object CloneObj()
    {
        return MemberwiseClone();
    }
}