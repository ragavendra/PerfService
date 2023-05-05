using Grpc.Core;
using PerfRunner.Network;

namespace PerfRunner.Services
{
    public class PerfService : Perf.PerfBase
    {
        private readonly ILogger<PerfService> _logger;
        private readonly IHttp _http;
        private readonly IGrpc _grpc;

        public PerfService(ILogger<PerfService> logger, IHttp http, IGrpc grpc)
        {
           _logger = logger;
           _http = http;
           _grpc = grpc;
        }

        public override Task<TestReply> RunTest(TestRequest testRequest, ServerCallContext context)
        {
            _logger.LogInformation("Message from Http service - " + _http.SampleHttpMethod());
            _logger.LogInformation("Message from Grpc service - " + _grpc.SampleGrpcMethod());

            return Task.FromResult(new TestReply
            {
                Message = $"Hi {testRequest.Name}"
            });
        }
        
    }
}