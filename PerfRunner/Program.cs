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
        /*
         var logDirName = $"{DateTime.UtcNow:yyyy.MM.dd_HH.mm.ss}";
         var logOutputDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "logs", logDirName));
         if (!Directory.Exists(logOutputDir))
         {
            Directory.CreateDirectory(logOutputDir);
         }*/


         CreateHostBuilder(args).Build().Run();
      }

      public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webHostBuilder =>
                     {
                        webHostBuilder.UseStartup<AppStart>();
                     });

   }
}