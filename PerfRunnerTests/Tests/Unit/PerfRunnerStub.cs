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
using OpenTelemetry.Metrics;
using PerfRunnerTests.Hepers;
using Google.Protobuf.WellKnownTypes;

namespace PerfRunnerTests.Tests.Unit
{
   // These should be stub test(s).
   public class PerfRunnerStub : IClassFixture<PerfRunnerData>
   {
      private PerfRunnerData _perfRunnerData;

      public PerfRunnerStub(PerfRunnerData perfRunnerData)
      {
         _perfRunnerData = perfRunnerData;
      }
      
      [Fact]
      public async Task RunTestWhenTestIsRunning()
      {
         // Arrange
         var service = GetPerfService();

         // Act
         var res = await service.RunTest(new TestRequest() { Name = "Some" }, _perfRunnerData.TestServerCallContext);
         
         // Assert
         Assert.Equal("Hi Some returned - Seems the test  is already runing.", res.Message);
      }

      // Not working completely as cloneobj mocking needs to be setup correctly.
      // AppDomain.GetCurrentDomain is not same when running test(s)
      [Fact]
      public async Task MockRunTest()
      {
         // Arrange
         var service = GetPerfService();

         // var req = It.IsAny<TestRequest>();
         var testRequest = new TestRequest { Name = "Some", Guid = Guid.NewGuid().ToString(), Rate = 3 };
         testRequest.Actions.Add(new ActionOption() { Name = "Login", Guid = Guid.NewGuid().ToString(), Rate = 7, Duration = Duration.FromTimeSpan(TimeSpan.FromSeconds(30)) });
         _perfRunnerData.MockActionRunner.Setup(x => x.Guid).Returns(It.IsAny<Guid>());

         // Act
         var res = await service.RunTest(testRequest, _perfRunnerData.TestServerCallContext);
         
         // Assert
         Assert.Equal("Hi Some", res.Message);
      }

      // Not working completely as cloneobj mocking needs to be setup correctly.
      [Fact]
      public async Task MockRunParamsTest()
      {
         // Arrange
         _perfRunnerData.MockTestStateMgr.Setup(m => m.AddTest(It.IsAny<TestRequest>())).Returns(true);

         _perfRunnerData.MockActionRunner.Setup(m => m.StartActionsPerSecondAsync(It.IsAny<int>())).ReturnsAsync(It.IsAny<bool>());

         var service = GetPerfService();

         // var req = It.IsAny<TestRequest>();
         var testRequest = new TestRequest { Name = "Some", Guid = Guid.NewGuid().ToString(), Rate = 3 };
         testRequest.Actions.Add(new ActionOption() { Name = "Login" });

         // Act
         var res = await service.RunTest(testRequest, _perfRunnerData.TestServerCallContext);

         // Assert
         Assert.Equal("Hi Some", res.Message);
      }

      // Not working yet - extended protobuf class cannot be loaded here
      [Fact]
      public async Task MockStopTest()
      {
         // Arrange
         var testRequest = new TestRequest { Name = "Some", Guid = "aaa", Rate = 3 };

         // could not get partial ext class vars like CancellationTokenSource, ActionBlock
         // testRequest?.CancellationTokenSource.Cancel();
         var cts = new CancellationTokenSource();
         var callContext = TestServerCallContext.Create(cancellationToken: cts.Token);

         var stopTestRequest = new StopTestRequest { Guid = testRequest.Guid };
         _perfRunnerData.MockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>())).Returns(It.IsAny<TestRequest>);
         _perfRunnerData.MockTestStateMgr.Setup(m => m.RemoveTest(It.IsAny<string>())).Returns(true);

         var service = GetPerfService();

         // var serverCallContext = TestServerCallContext.Create();

         // var res_ = await service.RunTest(testRequest, callContext);

         // Act
         // var req = It.IsAny<TestRequest>();
         // var res = await service.StopTest(stopTestRequest, callContext);
         var res = await service.StopTest(stopTestRequest, callContext);
         // var res = await service.StopTest(new StopTestRequest(){ Guid = "ak398s" }, serverCallContext);

         // Assert
         Assert.Equal(false, res.Status);
      }

      [Fact]
      public async Task MockUpdateRate()
      {
         // Arrange
         var testRequest = new TestRequest { Name = "Some", Guid = "aaa", Rate = 3 };
         _perfRunnerData.MockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>())).Returns(testRequest);
         var service = GetPerfService();

         // Act
         var res = service.UpdateRate(new UpdateRateRequest(){ Rate = 6 }, _perfRunnerData.TestServerCallContext);

         // Assert
         Assert.Equal(true, res.Result.Status);
      }

      private PerfService GetPerfService()
      {

         return new PerfService(
            _perfRunnerData.MockLogger.Object,
            _perfRunnerData.MockTestStateMgr.Object,
            _perfRunnerData.MockActionRunner.Object,
            _perfRunnerData.MockTestBase.Object,
            _perfRunnerData.MockUserManager.Object,
            _perfRunnerData.MockMeter.Object,
            _perfRunnerData.MockConf.Object);
      }

      // having issues to return the actionrunner object
      [Theory]
      [InlineData(ActionOptionUpdated.Rate, "6")]
      [InlineData(ActionOptionUpdated.Paused, "true")]
      public async Task MockUpdateAction(ActionOptionUpdated actionOptionUpdated, string value)
      {
         // Arrange
         /* Mock<TestRequest> mockTestRequest = new Mock<TestRequest>();
         mockTestRequest.Setup(x => x.Name).Returns("Some");
         mockTestRequest.Setup(x => x.Guid).Returns("aaa");
         mockTestRequest.Setup(x => x.Rate).Returns(3);
         mockTestRequest.Setup(m => m.GetActionRunner(It.IsAny<string>())).Returns(_perfRunnerData.MockActionRunner.Object);*/

         var testRequest = new TestRequest { Name = "Some", Guid = "aaa", Rate = 3 };
         // testRequest.GetActionRunner(testRequest.Guid);

         // ILogger<ActionRunner<ITestBase>> log = new ();
         // var actionRunner = new ActionRunner<ITestBase>(log);

         _perfRunnerData.MockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>())).Returns(testRequest);
         // _perfRunnerData.MockTestStateMgr.Setup(m => m.GetTest(It.IsAny<string>()).
           // GetActionRunner(It.IsAny<string>())).Returns(_perfRunnerData.MockActionRunner.Object);

         var service = GetPerfService();

         // Act
         var res = service.UpdateAction(new UpdateActionRequest(){ ActionOptionUpdate = actionOptionUpdated, UpdateValue = value }, _perfRunnerData.TestServerCallContext);

         // Assert
         Assert.Equal(true, res.Result.Status);
      }


    }
}