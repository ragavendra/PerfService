using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class TestBase
   {
      public Guid Guid = Guid.NewGuid();

      public readonly ILogger<TestBase> _logger;

      public TestBase() { }

      public TestBase(ILogger<TestBase> logger)
      {
         _logger = logger;
      }

      public virtual void RunTest()
      {
         // _logger?.LogInformation($"Running {nameof(this.GetType)} now.");
         Console.WriteLine($"Running {GetType().Name} now.");
         // throw new NotImplementedException();
      }
   }
}