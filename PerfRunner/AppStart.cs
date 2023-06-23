using PerfRunner.Services;
using PerfRunner.Tests;
using static WebApp.V1.WebApp;

namespace PerfRunner
{
   public class AppStart
   {
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddGrpc();
         services.AddTransient<IActionRunner<ITestBase>, ActionRunner<ITestBase>>();
         services.AddSingleton<ITestStateManager, TestStateManager>();

         // string redisAddress = "redis-app:6379";
         string redisAddress = "172.17.0.2:6379";
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

         // add typed http client factory
         services.AddHttpClient<ITestBase, TestBase>(client =>
         {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

            client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-raga");
         });

         // web app cli where the test(s) aim to make gRPC calls, can be stubbed or mocked
         services.AddGrpcClient<WebAppClient>(client =>
            client.Address = new Uri("https://localhost:7234"));

         // support for cli calls
         services.AddGrpcReflection();
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