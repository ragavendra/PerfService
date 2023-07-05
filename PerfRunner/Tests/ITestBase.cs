using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.Services;
using static WebApp.V1.WebApp;

namespace PerfRunner.Tests
{
    public interface ITestBase
    {
      public CancellationToken CancellationToken { get; set; }

      public HttpClient HttpClient { get; set; }

      public WebAppClient GrpcClient { get; set; }

      public IUserManager UserManager { get; set; }

      public void RunTest(Guid guid, ILogger<PerfService> logger);
    }
}