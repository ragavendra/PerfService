using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class TestBase : ITestBase, IDisposable
   {
      public Guid Guid = Guid.NewGuid();

      public readonly ILogger<TestBase> _logger;

      public HttpClient _httpClient { get; set; } = null!;

      public TestBase() { }

      public TestBase(HttpClient httpClient)
      {
         _httpClient = httpClient;
      }

/*
      public TestBase(ILogger<TestBase> logger, HttpClient httpClient)
      {
         _logger = logger;
         _httpClient = httpClient;
      }*/

      public virtual void RunTest(Guid guid)
      {
         // _logger?.LogInformation($"Running {nameof(this.GetType)} now.");
         Console.WriteLine($"Running {GetType().Name} now for {guid}.");
         // throw new NotImplementedException();
      }

      public void Dispose() => _httpClient?.Dispose();
   }
}