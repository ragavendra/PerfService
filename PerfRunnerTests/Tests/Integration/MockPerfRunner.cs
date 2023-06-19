using PerfRunner.Services;
using PerfRunner.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
// using PerfRunner;
using PerfRunnerTests.Tests.Integration.Helpers;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PerfRunner;
using Grpc.Net.Client;
using PerfRunner.V1;
// using PerfRunner.V1;

namespace PerfRunnerTests.Tests.Integration
{
   public class MockPerfRunner : IntegrationBase
   {
      public MockPerfRunner(GrpcTestFixture<AppStart> fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
      {
         var mockTestBase = new Mock<ITestBase>();
         var mockUserManager = new Mock<IUserManager>();

         var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         /*
         mockActionRunner.Setup(
             m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<TimeSpan>());*/

         Fixture.ConfigureWebHost(builder =>
         {
            builder.ConfigureServices(services => 
            { 
                services.AddTransient((Type)mockActionRunner.Object); 
                services.AddSingleton(mockTestStateMgr.Object);
                services.AddSingleton(mockUserManager.Object);
                
                });
         });


      }

      #region snippet_RunTest
      [Fact]
        public async Task RunTest_MockGreeter_Success()
        {
            // Arrange
            var client = new Perf.PerfClient(Channel);

            // Act
            var response = await client.RunTestAsync(
                new TestRequest { Name = "Joe" });

            // Assert
            Assert.Equal("hi Joe", response.Message);
        }
        #endregion

   }
}