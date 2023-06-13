using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PerfLoader;
using PerfRunner.V1;
using PerfRunnerTests.Tests.Client.Helpers;
using Xunit;
// using PerfRunner.Services;

namespace PerfRunnerTests.Tests
{
    public class PerfLoaderMock
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
      // the ping reply is mocked here and is returned
      // Actually testing the Worker bg service only.
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

      [Fact]
      public async Task TypeTest()
      {
        var mock = new Mock<ILoveThisLibrary>();

        mock.Setup(library => library.DownloadExists("1.0.0")).Returns(false);

        var lovable = mock.Object;

        bool dload = lovable.DownloadExists("1.0.0");
        dload = lovable.DownloadExists("1.0.0.");

        // Assert
        // if call was made atleast once with that set of args
        mock.Verify(libr => libr.DownloadExists("1.0.0"), Times.AtMostOnce());
        // Mock.Get(lovable).Verify(libr => libr.DownloadExists())
         // Assert.Pass();
         // return rep_;
      }

      [Fact]
      public async Task TypeTestLinq()
      {
        ILoveThisLibrary lovable = Mock.Of<ILoveThisLibrary>(lon => lon.DownloadExists("1.0.0") == true);

        bool dload = lovable.DownloadExists("1.0.0");
        // dload = lovable.DownloadExists("1.0.0.");

        // Assert
        Assert.True(dload);

        Mock.Get(lovable).Verify(lib => lib.DownloadExists("1.0.0"));
        Mock.Get(lovable).Verify(lib => lib.DownloadExists("1.0.0"), Times.AtLeastOnce());
      }

    }
}