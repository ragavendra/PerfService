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

      public bool _disposed;

      public readonly ILogger<TestBase> _logger;

      public HttpClient _httpClient { get; set; } = null!;

      public WebAppClient _grpcClient { get; set; } = null!;

      public IUserManager UserManager { get; set; }

      public TestBase() { }

      public TestBase(HttpClient httpClient, WebAppClient webAppClient, IUserManager userManager)
      {
         _httpClient = httpClient;
         _grpcClient = webAppClient;
         UserManager = userManager;
      }

      public virtual void RunTest(Guid guid, ILogger<PerfService> logger)
      {
         logger?.LogInformation($"Running {nameof(this.GetType)} now.");
      }

      public void Dispose()
      {
         Dispose(false);

         // tell the GC to not dispose this?
         GC.SuppressFinalize(this);

      }

      public void Dispose(bool disposing)
      {
         if(_disposed)
         {
            return;
         }

         if(disposing)
         {
            //dispose mgd resources

         }

         //dispose un mgd resources
         _httpClient?.Dispose();
         // _grpcClient?.Dispose();

         _disposed = true;
      }
   }
}