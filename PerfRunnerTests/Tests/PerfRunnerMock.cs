using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PerfLoader;
using PerfRunner.V1;
using PerfRunnerTests.Tests.Client.Helpers;
using Xunit;

namespace PerfRunnerTests.Tests
{
    public class PerfRunnerMock
    {

      // [Test(Description = "Run a perf test in the mock Runner Service")]
      [Fact]
      public async Task RunPerfTestMock()
      {
          var testRequest = new TestRequest { Name = "FirstTest", Guid = Guid.NewGuid().ToString(), Rate = 3 };
          testRequest.Actions.Add(new ActionOption() { Name = "Login" });

          var mockReply = CallHelpers.CreateAsyncUnaryCall(new PingReply { Message = "Reply here" });

          var mockClient = new Mock<Perf.PerfClient>();
          mockClient.Setup(mock => mock.PingAsync(It.IsAny<PingRequest>(), null, null,
          CancellationToken.None)).Returns(mockReply);
      }

      // Worker is a bg srvc in client, which in turn makes the gRPC 
      // call to the PerfRunner and gets the respo
      [Fact]
      public async Task RunGreeterMock()
      {
          // Arrange 
          var mockReply = CallHelpers.CreateAsyncUnaryCall(new PingReply { Message = "Reply here" });

          var mockClient = new Mock<Perf.PerfClient>();
          mockClient.Setup(mock => mock.PingAsync(It.IsAny<PingRequest>(), null, null,
          CancellationToken.None)).Returns(mockReply);

          var mockRepo = new Mock<IGreetRepository>();

          var worker = new Worker(mockClient.Object, mockRepo.Object);

          // Act
          await worker.StartAsync(CancellationToken.None);

          //Assert
          mockRepo.Verify(v => v.SaveGreeting("Reply here"));


         // Assert.Pass();
         // return rep_;
      }

    }
}