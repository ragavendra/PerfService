using Grpc.Core;

namespace PerfRunner.Services
{
    public class PerfService : Perf.PerfBase
    {
        private readonly ILogger<PerfService> _logger;

        public PerfService(ILogger<PerfService> logger)
        {
           _logger = logger;
        }

        public override Task<TestReply> RunTest(TestRequest pingRequest, ServerCallContext context)
        {
            return Task.FromResult(new TestReply
            {
                Message = $"Hi {pingRequest.Name}"
            });
        }
        
    }
}