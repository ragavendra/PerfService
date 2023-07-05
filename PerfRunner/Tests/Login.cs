using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;
using System.Text.Json;
using PerfRunner.Models;
using PerfRunner.Services;
using static WebApp.V1.WebApp;
using Polly;
using Polly.CircuitBreaker;
using System.Diagnostics;
using Polly.Wrap;
using Polly.Fallback;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class Login : TestBase
   {
      public Guid Guid = Guid.NewGuid();

      private static AsyncPolicyWrap<string> _policyWrap;

      private static Stopwatch watch;

      private static volatile int _count = 0;

      private static bool _initializeOnce;

      private object _lock = new object();

      private CancellationTokenSource _cts;

      /*
            public Login(ILogger<TestBase> logger, HttpClient httpClient) : base(logger, httpClient)
            {
               // _logger = logger;
               // _httpClient = httpClient;
            }*/

      public Login(HttpClient httpClient, WebAppClient webApp, IUserManager userManager) : base(httpClient, webApp, userManager)
      {
         // _logger = logger;
         // _httpClient = httpClient;
      }

      /*
            private readonly ILogger<Login> _logger;

            public Login(ILogger<Login> logger)
            {
               _logger = logger;
            }*/

      public override async void RunTest(Guid guid, ILogger<PerfService> logger)
      {

            //Policy ies
            // int i = 0;
            int eventualSuccesses = 0;
            int retries = 0;
            int eventualFailuresDueToCircuitBreaking = 0;
            int eventualFailuresForOtherReasons = 0;

         if(!_initializeOnce)
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
                              logger.LogDebug(".Log,then retry: " + exception.Message, ConsoleColor.Yellow);
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
                              logger.LogDebug(
                                  ".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!",
                                  ConsoleColor.Magenta);
                              logger.LogDebug("..due to: " + ex.Message, ConsoleColor.Magenta);
                           },
                           onReset: () => logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again!", ConsoleColor.Magenta),
                           onHalfOpen: () => logger.LogDebug(".Breaker logging: Half-open: Next call is a trial!", ConsoleColor.Magenta)
                           );

               // Define a fallback policy: provide a nice substitute message to the user, if we found the circuit was broken.
               var fallbackForCircuitBreaker = Policy<string>
                  .Handle<BrokenCircuitException>()
                  .FallbackAsync(
                     fallbackValue: /* Demonstrates fallback value syntax */ "Please try again later [Fallback for broken circuit]",
                     onFallbackAsync: async b =>
                     {
                        watch.Stop();
                        logger.LogDebug("Fallback catches failed with: " + b.Exception.Message, ConsoleColor.Red);
                        logger.LogDebug(" (after " + watch.ElapsedMilliseconds + "ms)", ConsoleColor.Red);
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
                        logger.LogDebug("Fallback catches eventually failed with: " + e.Exception.Message, ConsoleColor.Red);
                        logger.LogDebug(" (after " + watch.ElapsedMilliseconds + "ms)", ConsoleColor.Red);
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

         logger?.LogDebug($"Running {GetType().Name} now for {guid}.");
         // Console.WriteLine($"Running {GetType().Name} now for {guid}.");

         var user = UserManager?.CheckOutUser(UserState.Ready);
         if (user != null)
         {
            logger?.LogDebug($"User is {user?.Email}.");

            var userId = 1;
            // Task<Todo[]> todos;

            _count++;

            // Manage the call according to the whole policy wrap
                    await _policyWrap.ExecuteAsync(async ct =>
                    {
                        var todos = await HttpClient.GetFromJsonAsync<Todo[]>(
                               $"todoss?userId={userId}", new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        // logger?.LogInformation($"Title for todo item is {todos[3].title}.", ConsoleColor.Cyan);
                        
                        // return await HttpClient.GetStringAsync("/api/values/" + "1");
                        string some(){ return ""; }
                        Func<string> someFunc = some;
                        return await new Task<string>(someFunc);

                    }, CancellationToken);

                    watch.Stop();
                    eventualSuccesses++;

                    // var res = todos.Result;
                     // [3].title;


            // var todos = await HttpClient.GetFromJsonAsync<Todo[]>(
              // $"todos?userId={userId}", new JsonSerializerOptions(JsonSerializerDefaults.Web));

            // Console.WriteLine($"Title for todo item is {todos[3].title}.");
            // logger?.LogInformation($"Title for todo item is {todos[3].title}.");
         }

         if(_initializeOnce)
         {
            Console.WriteLine("");
            Console.WriteLine("Total requests made                     : " + _count);
            Console.WriteLine("Requests which eventually succeeded     : " + eventualSuccesses);
            Console.WriteLine("Retries made to help achieve success    : " + retries);
            Console.WriteLine("Requests failed early by broken circuit : " + eventualFailuresDueToCircuitBreaking);
            Console.WriteLine("Requests which failed after longer delay: " + eventualFailuresForOtherReasons);
         }
      }
   }
   /*
      public class SomeContr : Controller
      {
         HttpClient httpClient1 = new HttpClient();

         public OkObjectResult SomeMethod()
         {
            return Ok(httpClient1?.GetStringAsync("/someep"));
         }
      }*/
}

