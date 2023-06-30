using OpenTelemetry;
using OpenTelemetry.Metrics;
using PerfRunner.Services;
using PerfRunner.Tests;
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
         services.AddGrpc();
         services.AddTransient<IActionRunner<ITestBase>, ActionRunner<ITestBase>>();
         services.AddSingleton<ITestStateManager, TestStateManager>();

         services.AddSingleton<IUserManager, UserManager>();

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
         });

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