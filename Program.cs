using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using PerfRunner.Network;

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

         var builder = WebApplication.CreateBuilder(args);
         // builder.Services.AddScoped<IHttp, Http>();

         // generic type
         // Type openGenType = typeof(List<>);
         // Type closedGenType = openGenType.MakeGenericType(typeof(string));
         // builder.Services.AddScoped(openGenType, provider => Activator.CreateInstance(closedGenType));

         // resolve other service dynamically at run time
         // builder.Services.AddSingleton<IServiceProvider>(provider => provider);

         var services = new ServiceCollection();
         services.AddScoped<IHttp, Http>();
         
         // services.AddScoped(openGenType, provider => Activator.CreateInstance(closedGenType));
         // services.AddSingleton<IServiceProvider>(provider => provider);
         services.AddTransient(provider => new Lazy<IHttp>(provider.GetRequiredService<IHttp>));
         services.AddTransient(provider => new Func<IHttp>(provider.GetRequiredService<IHttp>));

         var serviceProvider = services.BuildServiceProvider();

         var service = serviceProvider.GetRequiredService<IHttp>();
         service.SomeMethod();

        /*
         var list = (System.Collections.IList)serviceProvider.GetRequiredService(closedGenType);
         list.Add("one");
         list.Add("seven");
         list.Add("six");

         var serviceProvider_ = serviceProvider.GetRequiredService<IServiceProvider>();
         var service_ = serviceProvider_.GetRequiredService<IHttp>();
         service_.SomeMethod(); */

         var lazySrvc = serviceProvider.GetRequiredService<Lazy<IHttp>>();
         lazySrvc.Value.SomeMethod();

         var funcSrvc = serviceProvider.GetRequiredService<Func<IHttp>>();
         funcSrvc().SomeMethod();

         // builder.Services.AddGrpc();
         var app = builder.Build();

         app.Run();

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