using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;
using System.Text.Json;
using PerfRunner.Models;
using PerfRunner.Services;
using static WebApp.V1.WebApp;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class LoginGrpc : TestBase
   {
      public Guid Guid = Guid.NewGuid();

      /*
            public Login(ILogger<TestBase> logger, HttpClient httpClient) : base(logger, httpClient)
            {
               // _logger = logger;
               // _httpClient = httpClient;
            }*/

      public LoginGrpc(HttpClient httpClient, WebAppClient webApp, IUserManager userManager) : base(httpClient, webApp, userManager)
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
         logger?.LogDebug($"Running {GetType().Name} now for {guid}.");
         // Console.WriteLine($"Running {GetType().Name} now for {guid}.");

         try
         {

            var request = new WebApp.V1.PingRequest() { Name = "hi from Login" };
            // trying rpc to the webapp
            WebApp.V1.PingReply call = await GrpcClient.PingAsync(request);

            logger.LogDebug($"Reply from WebApp is {call.Message}");

         }
         catch (System.Exception)
         {

            // throw;
            logger.LogDebug($"Obviously here! not implemented yet");
         }

      }
   }
}

