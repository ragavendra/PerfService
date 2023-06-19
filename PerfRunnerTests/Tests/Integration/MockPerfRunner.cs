using PerfRunner.Services;
using PerfRunner.Tests;
using Moq;
using PerfRunnerTests.Tests.Integration.Helpers;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using PerfRunner;
using PerfRunner.V1;

namespace PerfRunnerTests.Tests.Integration
{
    // These should be complete mock test(s).
   public class MockPerfRunner : IntegrationBase
   {
      /*
            public PerfService(
           ILogger<PerfService> logger,
           ITestStateManager testStateManager,
           IActionRunner<ITestBase> actionRunner,
           ITestBase testBase,
           IUserManager userManager,
           IConfiguration configuration)
        {
           _logger = logger;
           _testStateManager = testStateManager;
           _actionRunner = actionRunner;
           _testbase = testBase;
           _testbase.UserManager = userManager;
           _configuration = configuration;
        }

      */
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
               services.AddSingleton(mockActionRunner.Object);
               services.AddSingleton(mockTestStateMgr.Object);
               services.AddSingleton(mockUserManager.Object);

               // services.AddTransient(mockTestBase.Object);                
               // services.AddHttpClient(mockTestBase.Object);

            });
         });
      }

      #region snippet_RunTest
      [Fact]
      public async Task RunTest_MockGreeter_Success()
      {
         // Arrange
         var client = new Perf.PerfClient(Channel);
         var testRequest = new TestRequest { Name = "Some", Guid = Guid.NewGuid().ToString(), Rate = 3 };
         testRequest.Actions.Add(new ActionOption() { Name = "Login" });

         // Act
         var response = await client.RunTestAsync(testRequest);

         Console.WriteLine("Resp is " + response.Message);

         // Assert
         Assert.Equal("hi Joe", response.Message);
      }
      #endregion

   }
}