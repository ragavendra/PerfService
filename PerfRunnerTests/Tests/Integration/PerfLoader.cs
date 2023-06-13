// extern alias CustomTypes;
using PerfRunner.V1;
using PerfLoader;
using Grpc.Net.Client;
using NUnit.Framework;

namespace PerfRunnerTests.Tests.Integration
{
   [TestFixture]
   public class PerfLoader_
   {
      private Perf.PerfClient? PerfClient { get; set; }

      [OneTimeSetUp]
      public void OneTimeSetup()
      {
         Console.WriteLine("In One time Setup now.");
         Console.WriteLine("Make sure the perf runner service is up and running");

         var host_ = "http://localhost:5277";
         var channel = GrpcChannel.ForAddress(host_);
         PerfClient = new Perf.PerfClient(channel);
         try
         {
            PingReply pingReply = PerfClient.Ping(new PingRequest() { Name = "Check availability!" });
         }
         catch (Exception ex)
         {
            Console.WriteLine("Perf runner service is not available, aborting test run." + ex.Message);
            PerfClient = null;
            // throw;
         }
      }

      [SetUp]
      public void Setup()
      {
         Console.WriteLine("In Setup now.");
      }

      [Test(Description = "Run a perf test in the Runner Service")]
      public async Task RunPerfTest()
      {
          var testRequest = new TestRequest { Name = "FirstTest", Guid = Guid.NewGuid().ToString(), Rate = 3 };
          testRequest.Actions.Add(new ActionOption() { Name = "Login" });

          // not wait for task to complete
          var rep = PerfClient.RunTestAsync(testRequest);
          Assert.IsTrue(rep.ResponseAsync.Result.Status, "Test was not able to be started.");

          var rep_ = await PerfClient.StopTestAsync(new StopTestRequest { Guid = testRequest.Guid });
          Assert.IsTrue(rep.ResponseAsync.Result.Status, $"Test { testRequest.Guid } was not able to be stopped.");

         // Assert.Pass();
         // return rep_;
      }

      [Test(Description = "Run a perf test in the mock Runner Service")]
      public async Task RunPerfTest_()
      {
          var testRequest = new TestRequest { Name = "FirstTest", Guid = Guid.NewGuid().ToString(), Rate = 3 };
          testRequest.Actions.Add(new ActionOption() { Name = "Login" });

/*
          var mockReply = CallHelpers.CreateAsyncUnaryCall(new PingReply { Message = "Reply here" });

          var mockClient = new Mock<Perf.PerfClient>();
          mockClient.Setup(mock => mock.PingAsync(It.IsAny<PingRequest>(), null, null,
          CancellationToken.None)).Returns(mockReply);
*/
          // not wait for task to complete
          var rep = PerfClient.RunTestAsync(testRequest);
          Assert.IsTrue(rep.ResponseAsync.Result.Status, "Test was not able to be started.");

          var rep_ = await PerfClient.StopTestAsync(new StopTestRequest { Guid = testRequest.Guid });
          Assert.IsTrue(rep.ResponseAsync.Result.Status, $"Test { testRequest.Guid } was not able to be stopped.");

         // Assert.Pass();
         // return rep_;
      }

      [Test]
      public void Test1()
      {
         Console.WriteLine("In test 1 now");
      }

      [Test]
      public void Test2()
      {
         Console.WriteLine("In test 2 now");
      }

      [TestCase(1, "someStr", 3)]
      public void Test3(int no, string str, int no_)
      {
         Console.WriteLine($"In test 3 now with params {no} {str} {no_}");
      }
   }
}