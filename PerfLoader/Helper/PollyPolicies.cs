using Polly;
using Polly.CircuitBreaker;
using System.Diagnostics;
using Polly.Wrap;

namespace PerfLoader.Helper;

public class PollyPolicies
{
    public const string GooglePolicyName = "googleApiCircuitBreaker";
    public const string GrpcPolicyName = "grpcCircuitBreaker";

    private readonly ILogger _logger;

    public PollyPolicies()
    {
    }

/*
    public PollyPolicies(ILogger<PollyPolicies> logger)
    {
        _logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> GetGoogleApiCircuitBreakerPolicy()
    {
        // The google API is called using a HttpClient, so we can use the handy
        // HandleTransientHttpError() call. But this will not catch timeouts!
        // Make sure to fine-tune these numbers for your specific use-case

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: 0.1,
                samplingDuration: TimeSpan.FromSeconds(20),
                minimumThroughput: 5,
                durationOfBreak: TimeSpan.FromSeconds(60),
                this.OnGoogleBreak,
                this.OnGoogleReset,
                this.OnGoogleHalfOpen);
    }*/

    public AsyncCircuitBreakerPolicy GetGrpcCircuitBreakerPolicy()
    {
        // The Grpc API throws quite a few different exceptions (about 5) depending
        // on what the problem is, so we're just going to handle any exception:

        return Policy.Handle<Exception>().AdvancedCircuitBreakerAsync(
            failureThreshold: 0.01,
            samplingDuration: TimeSpan.FromSeconds(5),
            minimumThroughput: 15,
            durationOfBreak: TimeSpan.FromSeconds(360),
            this.OnGrpcBreak,
            this.OnGrpcReset,
            this.OnGrpcHalfOpen);
    }

    public void OnGrpcReset()
    {
        _logger?.LogWarning("Grpc circuit closed, requests flow normally.");
        Console.WriteLine("Grpc circuit closed, requests flow normally.");
    }

    public void OnGrpcHalfOpen()
    {
        _logger?.LogWarning("Grpc circuit half open");
        Console.WriteLine("Grpc circuit half open");
    }

    public void OnGrpcBreak(Exception result, TimeSpan ts)
    {
        _logger?.LogWarning("Grpc circuit cut because {Exception}, " + 
            "so requests will not flow.", result);

        Console.WriteLine("Grpc circuit cut because {Exception}, " + 
            "so requests will not flow.", result);
    }

    public void OnGoogleReset()
    {
        _logger?.LogWarning("Google API circuit closed, requests flow normally.");
        Console.WriteLine("Google API circuit closed, requests flow normally.");
    }

    public void OnGoogleHalfOpen()
    {
        _logger?.LogWarning("Google API circuit half open");
        Console.WriteLine("Google API circuit half open");
    }

    public void OnGoogleBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
    {
        _logger?.LogWarning("Google API circuit cut because {ResultStatusCode} " +
            "or {Exception}, so requests will not flow.",
            result.Exception, result.Result?.StatusCode);

       Console.WriteLine("Google API circuit cut because {ResultStatusCode} " +
            "or {Exception}, so requests will not flow.",
            result.Exception, result.Result?.StatusCode); 
    }
}
