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

      private HttpClient _httpClient;

      private WebAppClient _grpcClient;

      private IUserManager _userManager;

      private readonly Action<Guid, ILogger<PerfService>> _runTest;

      public IUserManager UserManager { get { return _userManager; } set { _userManager = value; } }

      public HttpClient HttpClient { get { return _httpClient; } set { _httpClient = value; } }

      public WebAppClient GrpcClient { get { return _grpcClient; } set { _grpcClient = value; } }

      public Action<Guid, ILogger<PerfService>> RunTest_ { get => _runTest; }

      public CancellationToken CancellationToken { get; set; }
      
      public Func<string, string> RunTest__ { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      // public TestBase() { }
      public TestBase(HttpClient httpClient, WebAppClient webAppClient, IUserManager userManager)
      {
         _httpClient = httpClient;
         _grpcClient = webAppClient;
         _userManager = userManager;
         _runTest = RunTest;
      }

      public virtual void RunTest(Guid guid, ILogger<PerfService> logger)
      {
         logger?.LogDebug($"Running {nameof(this.GetType)} now.");
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