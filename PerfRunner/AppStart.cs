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
         services.AddSingleton<IUserManager, UserManager>();

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