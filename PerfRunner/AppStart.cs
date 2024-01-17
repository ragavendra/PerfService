using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using PerfRunner.Models;
using PerfRunner.Services;
using PerfRunner.Tests;
using Polly;
using Polly.Extensions.Http;
using static WebApp.V1.WebApp;

namespace PerfRunner
{
   public class AppStart
   {
      public AppStart(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      public void ConfigureServices(IServiceCollection services)
      {

         var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
               {
                  TimeSpan.FromSeconds(1),
                  TimeSpan.FromSeconds(5),
                  TimeSpan.FromSeconds(10)
               });
         var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

         var circuitBreaker = HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(60));
         var bulkHead = Policy.BulkheadAsync(6).AsAsyncPolicy<HttpResponseMessage>();

         services.AddGrpc();
         services.AddTransient<IActionRunner<ITestBase>, ActionRunner<ITestBase>>();
         services.AddSingleton<ITestStateManager, TestStateManager>();

         // services.AddSingleton<IUserManager, UserManager>();

         services.AddSingleton<KafkaClientHandle>();
         services.AddSingleton<KafkaDependentProducer<string, User>>();
         // services.AddSingleton<KafkaDependentProducer<string, long>>();
         services.AddSingleton<IUserManager, KafkaUserStore>();

         /*
                  try
                  {

                     // string redisAddress = "redis-app:6379";
                     // string redisAddress = "172.17.0.2:6379";
                     string redisAddress = Configuration["REDIS_ADDR"];
                     RedisUserStore cartStore = null;
                     if (string.IsNullOrEmpty(redisAddress))
                     {
                        Console.WriteLine("REDIS_ADDR environment variable is required.");
                        Environment.Exit(1);
                     }

                     cartStore = new RedisUserStore(redisAddress);

                     // Initialize the redis store
                     cartStore.InitializeAsync();
                     Console.WriteLine("Initialization completed");
                     // cartStore = null;

                     services.AddSingleton<IUserManager>(cartStore);
                  }
                  catch (Exception ex)
                  {
                     Console.WriteLine("Siome issue with redis, using in memory User Manager instead.");
                  }*/

         // add typed http client factory
         services.AddHttpClient<ITestBase, TestBase>(client =>
         {
            client.BaseAddress = new Uri(Configuration["HTTP_APP1"]);

            client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-raga");
         })

         // since non Get calls can be idempotent, using retry only for gets
            .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy);
         /*
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
               new[]
                  {
                     TimeSpan.FromSeconds(1),
                     TimeSpan.FromSeconds(5),
                     TimeSpan.FromSeconds(10)
                  },
               (exception, timeSpan, retryCount, context) => Console.WriteLine($"Some issue with http conn - {exception.Result}")))
            .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
               handledEventsAllowedBeforeBreaking: 3,
               durationOfBreak: TimeSpan.FromSeconds(30)
            ));*/

         /*
                  builder.Services.AddHttpClient<PerfService>(client => {
                     client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                     client.DefaultRequestHeaders.UserAgent.ParseAdd("dottnet-raga-perfSrvc");
                  });*/


         // web app cli where the test(s) aim to make gRPC calls, can be stubbed or mocked
         services.AddGrpcClient<WebAppClient>(client =>
            client.Address = new Uri(Configuration["GRPC_APP1"]));

         // support for cli calls
         services.AddGrpcReflection();

         // instr
         MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter(Configuration["INSTR_METER"])
            .AddPrometheusExporter(opt =>
            {
               opt.StartHttpListener = true;
               opt.HttpListenerPrefixes = new string[] { Configuration["INSTR_LISTENER"] };
            })
            .Build();

         services.AddSingleton(meterProvider);
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseRouting();

         app.UseEndpoints(endponts =>
         {
            endponts.MapGrpcService<PerfService>();
            endponts.MapGet("/", () => "Comm with gRPC should be made through gRPC clients.");

            endponts.MapGrpcReflectionService();
         });
      }
   }
}
