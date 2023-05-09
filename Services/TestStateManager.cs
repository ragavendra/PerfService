using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Services
{
   // Static class to maintain or manage test(s).
   public class TestStateManager : Perf.PerfBase
   {
      public Guid Guid = Guid.NewGuid();

      // lets have test guid and its options here
      public readonly ConcurrentDictionary<Guid, TestRequest> Tests = new ConcurrentDictionary<Guid, TestRequest>();

      private readonly ILogger<TestStateManager> _logger;

      //return the first test
      public TestRequest? GetTest(string guid) => Tests.First(test => test.Key.ToString().
         Equals(guid)).Value;

      public TestStateManager(ILogger<TestStateManager> logger)
      {
         _logger = logger;
      }

      public bool AddTest(TestRequest testRequest)
      {
         return Tests.TryAdd(Guid.Parse(testRequest.Guid), testRequest);
      }

      public bool RemoveTest(string guid)
      {
         return Tests.TryRemove(Guid.Parse(guid), out _);
      }

   }
}