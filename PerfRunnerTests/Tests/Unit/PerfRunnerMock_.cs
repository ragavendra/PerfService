// using PerfRunner.Services;
// using PerfRunner.Tests;
using PerfRunner.V1;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using PerfRunnerTests.Tests.Unit.Helpers;

namespace PerfRunnerTests.Tests.Unit
{
   /*
   public class PerfRunnerMock_
   {
            // Not working yet
      [Fact]
      public async Task MockStopTest()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();
         var testRequest = new TestRequest { Name = "Some", Guid = "aaa", Rate = 3 };
         // testRequest.Can
         
         // testRequest.
         // could not get partial ext class vars like CancellationTokenSource, ActionBlock
         // testRequest?.CancellationTokenSource.Cancel();
         var cts = new CancellationTokenSource();
         var callContext = TestServerCallContext.Create(cancellationToken: cts.Token);

         var stopTestRequest = new StopTestRequest { Guid = testRequest.Guid };
         mockTestStateMgr.Setup(m => m.GetTest(testRequest.Guid)).Returns(testRequest);
         mockTestStateMgr.Setup(m => m.RemoveTest(testRequest.Guid)).Returns(true);

         // var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         // mockActionRunner.Setup(m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<TimeSpan>());

         var mockLogger = new Mock<ILogger<PerfService>>();

         var mockTestBase = new Mock<ITestBase>();

         var mockUserManager = new Mock<IUserManager>();

         var mockConf = new Mock<IConfiguration>();

         var service = new PerfService(
            mockLogger.Object,
            mockTestStateMgr.Object,
            mockActionRunner.Object,
            mockTestBase.Object,
            mockUserManager.Object,
            mockConf.Object);

         var httpContext = new DefaultHttpContext();
         // var serverCallContext = TestServerCallContext.Create();

         // var res_ = await service.RunTest(testRequest, callContext);

         // Act
         // var req = It.IsAny<TestRequest>();
         var res = await service.StopTest(stopTestRequest, callContext);
         // var res = await service.StopTest(new StopTestRequest(){ Guid = "ak398s" }, serverCallContext);

         // Assert
         Assert.Equal(false, res.Status);
      }*/

    }
}