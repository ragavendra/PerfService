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
   public class PlayBowling : TestBase
   {
      public Guid Guid = Guid.NewGuid();

      /*
            public Login(ILogger<TestBase> logger, HttpClient httpClient) : base(logger, httpClient)
            {
               // _logger = logger;
               // _httpClient = httpClient;
            }*/

      public PlayBowling(HttpClient httpClient, WebAppClient webApp, IUserManager userManager) : base(httpClient, webApp, userManager)
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

         // get auth user 
         var user = UserManager?.CheckOutUser(UserState.Authenticated);

         if(user != null)
         {
            logger?.LogDebug($"User is {user?.Email}.");

            var userId = 1;
            var todos = await _httpClient.GetFromJsonAsync<Todo[]>(
               $"todos?userId={userId}", new JsonSerializerOptions(JsonSerializerDefaults.Web));

            // replace with actual http or rpc call to check lane availability
            // if available set to playing
            var rndFlag = (new Random()).Next();

            if (rndFlag % 2 == 0)
            {
               user.State = UserState.Playing;
            }
            else
            {
               user.State = UserState.Waiting;
            }

            UserManager?.CheckInUser(user);
            // logger?.LogDebug($"User is {user?.Email}.");
         }
      }
   }
}

