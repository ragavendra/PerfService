
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OpenTelemetry.Metrics;
using PerfRunner.Services;
using PerfRunner.Tests;

namespace PerfRunnerTests.Hepers;

public class PerfRunnerData
{
    private PerfService _perfService;

    public PerfService PerfService { get { return _perfService; } }

    public PerfRunnerData()
    {

         // var mockTestStateMgr = new Mock<ITestStateManager>();
         var mockTestStateMgr = new Mock<ITestStateManager>();

         // mockTestStateMgr.Setup(m => m.AddTest(It.IsAny<TestRequest>())).Returns(false);
         // var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         var mockActionRunner = new Mock<IActionRunner<ITestBase>>();
         // mockActionRunner.Setup(m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<TimeSpan>());

         var mockLogger = new Mock<ILogger<PerfService>>();

         var mockTestBase = new Mock<ITestBase>();

         var mockUserManager = new Mock<IUserManager>();

         var mockMeter = new Mock<MeterProvider>();

         var mockConf = new Mock<IConfiguration>();

         _perfService = new PerfService(
            mockLogger.Object,
            mockTestStateMgr.Object,
            mockActionRunner.Object,
            mockTestBase.Object,
            mockUserManager.Object,
            mockMeter.Object,
            mockConf.Object);
    }

}