using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.Services;
using PerfRunner.V1;
using static WebApp.V1.WebApp;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class TestBase : ITestBase, IDisposable
   {
      public Guid Guid = Guid.NewGuid();

      public readonly ILogger<TestBase> _logger;

      public HttpClient _httpClient { get; set; } = null!;

      public WebAppClient _grpcClient { get; set; } = null!;

      public UserManager UserManager { get; set; }

      public TestBase() { }

      public TestBase(HttpClient httpClient, WebAppClient webAppClient, UserManager userManager)
      {
         _httpClient = httpClient;
         _grpcClient = webAppClient;
         UserManager = userManager;
      }

/*
      public TestBase(ILogger<TestBase> logger, HttpClient httpClient)
      {
         _logger = logger;
         _httpClient = httpClient;
      }*/

      public virtual void RunTest(Guid guid, ILogger<PerfService> logger)
      {
         logger?.LogInformation($"Running {nameof(this.GetType)} now.");
         // Console.WriteLine($"Running {GetType().Name} now for {guid}.");
         // throw new NotImplementedException();
      }

      public void Dispose() => _httpClient?.Dispose();
   }
}