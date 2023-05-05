using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace PerfRunner
{
    public static class Program
   {

      private static X509Certificate2 _serverCertificate;

      /// <summary>
      /// Sample.
      /// </summary>
      public static async Task Main(string[] args)
      {
         var logDirName = $"{DateTime.UtcNow:yyyy.MM.dd_HH.mm.ss}";
         var logOutputDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "logs", logDirName));
         if (!Directory.Exists(logOutputDir))
         {
            Directory.CreateDirectory(logOutputDir);
         }
      }

      /// <summary>Sample.</summary>
      public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(
              webBuilder =>
              {
                 webBuilder
                    .ConfigureKestrel(
                       options => options.ConfigureHttpsDefaults(
                          adapterOptions =>
                          {
                              adapterOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                              adapterOptions.ClientCertificateValidation +=
                              (certificate, chain, errors) => certificate.Thumbprint == _serverCertificate.Thumbprint;
                              adapterOptions.ServerCertificate = _serverCertificate;
                           }))
                    .UseUrls("https://*:9001") // default, can be overridden by command line, e.g. --urls "https://*:1234"
                    .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                 // .UseStartup<Startup>()
                    .ConfigureLogging(logging => logging.ClearProviders().SetMinimumLevel(LogLevel.Trace));
                 // .UseNLog();
              });
   }
}