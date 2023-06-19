using Xunit;
using Xunit.Abstractions;
using Grpc.Net.Client;
using PerfRunner;
using Microsoft.Extensions.Logging;
using PerfRunnerTests.Tests.Integration.Helpers;

namespace PerfRunnerTests.Tests.Integration
{
   public class IntegrationBase : IClassFixture<GrpcTestFixture<AppStart>>, IDisposable
   {
      private GrpcChannel? _channel;
      private IDisposable? _testContext;

      protected GrpcTestFixture<AppStart> Fixture { get; set; }

      protected ILoggerFactory LoggerFactory => Fixture.LoggerFactory;

      protected GrpcChannel Channel => _channel ??= CreateChannel();

      protected GrpcChannel CreateChannel()
      {
         return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
         {
            LoggerFactory = LoggerFactory,
            HttpHandler = Fixture.Handler
         });
      }

      public IntegrationBase(GrpcTestFixture<AppStart> fixture, ITestOutputHelper outputHelper)
      {
         Fixture = fixture;
         _testContext = Fixture.GetTestContext(outputHelper);
      }

      public void Dispose()
      {
         _testContext?.Dispose();
         _channel = null;
      }
   }
}