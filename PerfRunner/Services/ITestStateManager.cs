using System.Collections.Concurrent;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public interface ITestStateManager
    {
        public ConcurrentDictionary<Guid, TestRequest> Tests { get; set; }

        //return the first test
        public TestRequest? GetTest(string guid);

        public bool AddTest(TestRequest testRequest);

        public bool RemoveTest(string guid);
    }
}