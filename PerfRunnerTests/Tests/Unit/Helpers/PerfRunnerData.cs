
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OpenTelemetry.Metrics;
using PerfRunner.Services;
using PerfRunner.Tests;
using PerfRunnerTests.Tests.Unit.Helpers;

namespace PerfRunnerTests.Hepers;

public class PerfRunnerData
{
    private readonly PerfService _perfService;

    private readonly DefaultHttpContext _defaultHttpContext;

    private readonly TestServerCallContext _testServerCallContext;

    private readonly Mock<ITestStateManager> _mockTestStateMgr;

    private readonly Mock<IActionRunner<ITestBase>> _mockActionRunner;

    private readonly Mock<ILogger<PerfService>> _mockLogger;

    private readonly Mock<ITestBase> _mockTestBase;

    private readonly Mock<IUserManager> _mockUserManager;

    private readonly Mock<MeterProvider> _mockMeter;

    private readonly Mock<IConfiguration> _mockConf;

    public Mock<ITestStateManager> MockTestStateMgr { get => _mockTestStateMgr; }

    public Mock<IActionRunner<ITestBase>> MockActionRunner { get => _mockActionRunner; }

    public Mock<ILogger<PerfService>> MockLogger { get => _mockLogger; }

    public Mock<ITestBase> MockTestBase { get => _mockTestBase; }

    public Mock<IUserManager> MockUserManager { get => _mockUserManager; }

    public Mock<MeterProvider> MockMeter { get => _mockMeter; }

    public Mock<IConfiguration> MockConf { get  => _mockConf; }

    public DefaultHttpContext DefaultHttpContext { get => _defaultHttpContext; }

    public TestServerCallContext TestServerCallContext { get => _testServerCallContext; }

    public PerfRunnerData()
    {

         // var mockTestStateMgr = new Mock<ITestStateManager>();
         _mockTestStateMgr = new Mock<ITestStateManager>();

         _mockActionRunner = new Mock<IActionRunner<ITestBase>>();

         _mockLogger = new Mock<ILogger<PerfService>>();

         _mockTestBase = new Mock<ITestBase>();

         _mockUserManager = new Mock<IUserManager>();

         _mockMeter = new Mock<MeterProvider>();

         _mockConf = new Mock<IConfiguration>();

         _defaultHttpContext = new DefaultHttpContext();
         _testServerCallContext = TestServerCallContext.Create();
    }

}