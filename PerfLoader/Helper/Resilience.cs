using Polly;
using Polly.CircuitBreaker;
using System.Diagnostics;
using Polly.Wrap;

namespace PerfLoader.Helper;

public class Resilience
{
    private static AsyncPolicyWrap<string> _policyWrap;

    private static Stopwatch watch;

    private static volatile int _count = 0;

    private static bool _initializeOnce;

    private static object _lock = new object();

    private static CancellationTokenSource _cts;

    public static AsyncPolicyWrap<string> PolicyWrap { get => _policyWrap; }

    public static void LoadPolicy<T>(ILogger<T> logger)
    {
        //Policy ies
        // int i = 0;
        int eventualSuccesses = 0;
        int retries = 0;
        int eventualFailuresDueToCircuitBreaking = 0;
        int eventualFailuresForOtherReasons = 0;

        if (!_initializeOnce)
        {
            lock (_lock)
            {
                watch = null;
                _cts = new CancellationTokenSource();

                // Define our waitAndRetry policy: keep retrying with 200ms gaps.
                var waitAndRetryPolicy = Policy
                      .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                      .WaitAndRetryForeverAsync(
                            attempt => TimeSpan.FromMilliseconds(200),
                            (exception, calculatedWaitDuration) =>
                            {
                                Console.WriteLine(".Log,then retry: " + exception.Message, ConsoleColor.Yellow);
                                retries++;
                            });

                // Define our CircuitBreaker policy: Break if the action fails 4 times in a row.
                var circuitBreakerPolicy = Policy
                      .Handle<Exception>()
                      .CircuitBreakerAsync(
                            exceptionsAllowedBeforeBreaking: 4,
                            durationOfBreak: TimeSpan.FromSeconds(3),
                            onBreak: (ex, breakDelay) =>
                            {
                                Console.WriteLine(
                                   ".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!",
                                   ConsoleColor.Magenta);
                                Console.WriteLine("..due to: " + ex.Message, ConsoleColor.Magenta);
                            },
                            onReset: () => Console.WriteLine(".Breaker logging: Call ok! Closed the circuit again!", ConsoleColor.Magenta),
                            onHalfOpen: () => Console.WriteLine(".Breaker logging: Half-open: Next call is a trial!", ConsoleColor.Magenta)
                            );

                // Define a fallback policy: provide a nice substitute message to the user, if we found the circuit was broken.
                var fallbackForCircuitBreaker = Policy<string>
                   .Handle<BrokenCircuitException>()
                   .FallbackAsync(
                      fallbackValue: /* Demonstrates fallback value syntax */ "Please try again later [Fallback for broken circuit]",
                      onFallbackAsync: async b =>
                      {
                          watch.Stop();
                          Console.WriteLine("Fallback catches failed with: " + b.Exception.Message, ConsoleColor.Red);
                          Console.WriteLine(" (after " + watch.ElapsedMilliseconds + "ms)", ConsoleColor.Red);
                          eventualFailuresDueToCircuitBreaking++;

                          _cts.Cancel();
                      }
                      );

                // Define a fallback policy: provide a substitute string to the user, for any exception.
                var fallbackForAnyException = Policy<string>
                   .Handle<Exception>()
                   .FallbackAsync(
                      fallbackAction: /* Demonstrates fallback action/func syntax */ async ct =>
                      {
                          /* do something else async if desired */
                          return "Please try again later [Fallback for any exception]";
                      },
                      onFallbackAsync: async e =>
                      {
                          watch.Stop();
                          Console.WriteLine("Fallback catches eventually failed with: " + e.Exception.Message, ConsoleColor.Red);
                          Console.WriteLine(" (after " + watch.ElapsedMilliseconds + "ms)", ConsoleColor.Red);
                          eventualFailuresForOtherReasons++;
                      }
                      );

                // As demo07: we combine the waitAndRetryPolicy and circuitBreakerPolicy into a PolicyWrap, using the *static* Policy.Wrap syntax.
                AsyncPolicyWrap myResilienceStrategy = Policy.WrapAsync(waitAndRetryPolicy, circuitBreakerPolicy);

                // Added in demo08: we wrap the two fallback policies onto the front of the existing wrap too.  Demonstrates the *instance* wrap syntax. And the fact that the PolicyWrap myResilienceStrategy from above is just another Policy, which can be onward-wrapped too.  
                // With this pattern, you can build an overall resilience strategy programmatically, reusing some common parts (eg PolicyWrap myResilienceStrategy) but varying other parts (eg Fallback) individually for different calls.
                _policyWrap = fallbackForAnyException.WrapAsync(fallbackForCircuitBreaker.WrapAsync(myResilienceStrategy));
                // For info: Equivalent to: PolicyWrap<String> policyWrap = Policy.Wrap(fallbackForAnyException, fallbackForCircuitBreaker, waitAndRetryPolicy, circuitBreakerPolicy);

                watch = new Stopwatch();
                watch.Start();

                _initializeOnce = true;
            }
        }
    }
}