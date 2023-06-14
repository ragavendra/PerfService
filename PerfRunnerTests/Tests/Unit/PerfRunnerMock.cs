extern alias CustomTypes;
using CustomTypes.PerfRunner.Services;
using CustomTypes.PerfRunner.Tests;
using CustomTypes.PerfRunner.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
// using PerfRunner.V1;
using PerfRunnerTests.Tests.Client.Helpers;
using Xunit;
// using PerfRunner.Services;
// using PerfRunner.Tests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using PerfRunnerTests.Tests.Unit.Helpers;

namespace PerfRunnerTests.Tests.Unit
{
   public class PerfRunnerMock
   {
      [Fact]
      public async Task RunTestWhenTestIsRunning()
      {
         // Arrange
         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();

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
         // Act
         var res = await service.RunTest(new TestRequest(){ Name = "Some" }, serverCallContext);
         
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



      [Fact]
      public async Task TestStateMgrMock()
      {
        // Arrange
        var mockTestStateMgr = new Mock<ITestStateManager>();
        // mockTestStateMgr.Setup(m => m.AddTest(It.IsAny<string>())).Returns(true);

        var mockActionRunner = new Mock<IActionRunner<Login>>();
        mockActionRunner.Setup(m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<TimeSpan>());

        // var service = new PerfService(new ILogger<PerfService>(), mockTestStateMgr.Object, mockActionRunner.Object);
      }

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
         var res = await service.UpdateRate(new UpdateRateRequest(){ Rate = 6 }, serverCallContext);

         // Assert
         Assert.Equal(true, res.Status);
      }


    }
}