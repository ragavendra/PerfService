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
   public class Login : TestBase
   {
      public Guid Guid = Guid.NewGuid();

      /*
            public Login(ILogger<TestBase> logger, HttpClient httpClient) : base(logger, httpClient)
            {
               // _logger = logger;
               // _httpClient = httpClient;
            }*/

      public Login(HttpClient httpClient, WebAppClient webApp, UserManager userManager) : base(httpClient, webApp, userManager)
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
         logger?.LogInformation($"Running {GetType().Name} now for {guid}.");
         // Console.WriteLine($"Running {GetType().Name} now for {guid}.");

         var user = UserManager?.CheckOutUser(UserState.Ready);
         if (user != null)
         {
            logger?.LogInformation($"User is {user?.Email}.");

            var userId = 1;
            var todos = await _httpClient.GetFromJsonAsync<Todo[]>(
               $"todos?userId={userId}", new JsonSerializerOptions(JsonSerializerDefaults.Web));

            // Console.WriteLine($"Title for todo item is {todos[3].title}.");
            logger?.LogInformation($"Title for todo item is {todos[3].title}.");

            try
            {

               var request = new WebApp.V1.PingRequest() { Name = "hi from Login" };
               // trying rpc to the webapp
               // WebApp.V1.PingReply call = await _grpcClient.PingAsync(request);

               // logger.LogInformation($"Reply from WebApp is {call.Message}");

            }
            catch (System.Exception)
            {

               // throw;
               logger.LogInformation($"Obviously here! not implemented yet");
            }

            user.State = UserState.Authenticated;
            UserManager?.CheckInUser(user);
            // logger?.LogInformation($"User is {user?.Email}.");
         }

      }
   }
}

