using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class TestBase : ITestBase
   {
      public Guid Guid = Guid.NewGuid();

      public readonly ILogger<TestBase> _logger;

      public TestBase() { }

      public TestBase(ILogger<TestBase> logger)
      {
         _logger = logger;
      }

      public virtual void RunTest(Guid guid)
      {
         // _logger?.LogInformation($"Running {nameof(this.GetType)} now.");
         Console.WriteLine($"Running {GetType().Name} now for {guid}.");
         // throw new NotImplementedException();
      }
   }
}