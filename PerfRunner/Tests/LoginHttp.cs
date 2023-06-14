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
   public class LoginHttp : TestBase
   {
      public Guid Guid = Guid.NewGuid();

      /*
            public Login(ILogger<TestBase> logger, HttpClient httpClient) : base(logger, httpClient)
            {
               // _logger = logger;
               // _httpClient = httpClient;
            }*/

      public LoginHttp(HttpClient httpClient, WebAppClient webApp, IUserManager userManager) : base(httpClient, webApp, userManager)
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

         var userId = 1;
         var todos = await _httpClient.GetFromJsonAsync<Todo[]>(
            $"todos?userId={userId}", new JsonSerializerOptions(JsonSerializerDefaults.Web));

         // Console.WriteLine($"Title for todo item is {todos[3].title}.");
         logger?.LogDebug($"Title for todo item is {todos[3].title}.");

      }
   }
}

