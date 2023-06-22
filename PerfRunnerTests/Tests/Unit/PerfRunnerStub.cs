using Moq;
using Xunit;
// using PerfRunner.Services;
// using PerfRunner.Tests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using PerfRunnerTests.Tests.Unit.Helpers;
using PerfRunner.Services;
using PerfRunner.Tests;
using PerfRunner.V1;

namespace PerfRunnerTests.Tests.Unit
{
   // These should be stub test(s).
   public class PerfRunnerStub
   {
      [Fact]
      public async Task RunTestWhenTestIsRunning()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();

         // mockTestStateMgr.Setup(m => m.AddTest(It.IsAny<TestRequest>())).Returns(false);
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
         var serverCallContext = TestServerCallContext.Create();

         // Act
         var res = await service.RunTest(new TestRequest() { Name = "Some" }, serverCallContext);
         
         // Assert
         Assert.Equal("Hi Some returned - Seems the test  is already runing.", res.Message);
      }

      // Not working completely as cloneobj mocking needs to be setup correctly.
      // AppDomain.GetCurrentDomain is not same when running test(s)
      [Fact]
      public async Task MockRunTest()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();
         mockTestStateMgr.Setup(m => m.AddTest(It.IsAny<TestRequest>())).Returns(true);

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
         var serverCallContext = TestServerCallContext.Create();

         // var req = It.IsAny<TestRequest>();
         var testRequest = new TestRequest { Name = "Some", Guid = Guid.NewGuid().ToString(), Rate = 3 };
         testRequest.Actions.Add(new ActionOption() { Name = "Login" });

         // Act
         var res = await service.RunTest(testRequest, serverCallContext);
         
         // Assert
         Assert.Equal("Hi Some", res.Message);
      }

      // Not working completely as cloneobj mocking needs to be setup correctly.
      [Fact]
      public async Task MockRunParamsTest()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();
         mockTestStateMgr.Setup(m => m.AddTest(It.IsAny<TestRequest>())).Returns(true);

         // var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         mockActionRunner.Setup(m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<TimeSpan>());
         // mockActionRunner.Setup(m => m.CloneObj()).Returns(It.IsAny<IActionRunner<ITestBase>>());

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
         var serverCallContext = TestServerCallContext.Create();

         // var req = It.IsAny<TestRequest>();
         var testRequest = new TestRequest { Name = "Some", Guid = Guid.NewGuid().ToString(), Rate = 3 };
         testRequest.Actions.Add(new ActionOption() { Name = "Login" });

         // Act
         var res = await service.RunTest(testRequest, serverCallContext);

         // Assert
         Assert.Equal("Hi Some", res.Message);
      }

      // Not working yet - extended protobuf class cannot be loaded here
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
         // mockTestStateMgr.Setup(m => m.GetTest(testRequest.Guid)).Returns(testRequest);
         mockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>())).Returns(It.IsAny<TestRequest>);
         
         // mockTestStateMgr.Setup(m => m.GetTest(testRequest.Guid)?.CancellationTokenSource().Cancel).Returns(testRequest);
         mockTestStateMgr.Setup(m => m.RemoveTest(It.IsAny<string>())).Returns(true);

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
      }

      [Fact]
      public async Task MockUpdateRate()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();
         var testRequest = new TestRequest { Name = "Some", Guid = "aaa", Rate = 3 };
         mockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>())).Returns(testRequest);

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
         var serverCallContext = TestServerCallContext.Create();

         // Act
         var res = service.UpdateRate(new UpdateRateRequest(){ Rate = 6 }, serverCallContext);

         // Assert
         Assert.Equal(true, res.Status);
      }


    }
}