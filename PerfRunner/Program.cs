using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using PerfRunner.Network;
using PerfRunner.Services;
using PerfRunner.Tests;
using static WebApp.V1.WebApp;

namespace PerfRunner
{
    public static class Program
   {
      /// <summary>
      /// PE for the Perf Runner or Srvc.
      /// </summary>
      public static async Task Main(string[] args)
      {
         // using System;
         // using System.Linq;

/*
         using var db = new LoggingContext();

         // Note: This sample requires the database to be created before running.
         Console.WriteLine($"Database path: {db.DbPath}.");

         // Create
         Console.WriteLine("Inserting a new log");
         db.Add(new Log { Url = "http://blogs.msdn.com/adonet" });
         db.SaveChanges();

         // Read
         Console.WriteLine("Querying for a log");
         var blog = db.Logs
             .OrderBy(b => b.LogId);
             // .First();

             foreach(var entry in blog)
             {
               Console.WriteLine("Entry is: " + entry.Url);

             }

         // Update
         Console.WriteLine("Updating the blog and adding a post");
         blog.Last().Url = "https://devblogs.microsoft.com/dotnet";
         blog.Last().Entries.Add(
             new Entry { Title = "Hello World", Content = "I wrote an app using EF Core!" });
         db.SaveChanges();*/

         // Delete
         // Console.WriteLine("Delete the blog");
         // db.Remove(blog);
         // db.SaveChanges();
        /*
         var logDirName = $"{DateTime.UtcNow:yyyy.MM.dd_HH.mm.ss}";
         var logOutputDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "logs", logDirName));
         if (!Directory.Exists(logOutputDir))
         {
            Directory.CreateDirectory(logOutputDir);
         }*/

         var builder = WebApplication.CreateBuilder(args);

         builder.Services.AddGrpc();
         builder.Services.AddTransient<ActionRunner<ITestBase>>();
         builder.Services.AddSingleton<TestStateManager>();
         builder.Services.AddSingleton<UserManager>();

         builder.Services.AddSingleton<LoggingContext>();

         // add typed http client factory
         builder.Services.AddHttpClient<ITestBase, TestBase>(client => {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

            client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-raga");
         });

         // web app cli where the test(s) aim to make gRPC calls, can be stubbed or mocked 
         builder.Services.AddGrpcClient<WebAppClient>(client =>
            client.Address = new Uri("https://localhost:7234"));

         // support for cli calls
         builder.Services.AddGrpcReflection();

         var app = builder.Build();
         app.MapGrpcService<PerfService>();
         app.MapGet("/", () => "Comm with gRPC should be made through gRPC clients.");

         var env = app.Environment;

         if(env.IsDevelopment()){
            app.MapGrpcReflectionService();
         }

         app.Run();
      }

   }
}