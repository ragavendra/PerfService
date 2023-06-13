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

         var res = await service.RunTest(new TestRequest() { Name = "Some" }, serverCallContext);
         Assert.Equal("Hi Some returned - Seems the test  is already runing.", res.Message);
      }

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
         var res = await service.RunTest(new TestRequest(){ Name = "Some" }, serverCallContext);
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

    }
}