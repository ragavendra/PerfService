using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public class PingService : Perf.PerfBase
    {
        private readonly ILogger<PingService> _logger;

        public PingService(ILogger<PingService> logger)
        {
           _logger = logger;
        }

        public override Task<PingReply> Ping(PingRequest pingRequest, ServerCallContext context)
        {
            return Task.FromResult(new PingReply
            {
                Message = $"Hi {pingRequest.Name}"
            });
        }
        
    }
}