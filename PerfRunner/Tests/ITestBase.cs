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
      public HttpClient _httpClient { get; set; }

      public WebAppClient _grpcClient { get; set; }

      public UserManager UserManager { get; set; }

      public void RunTest(Guid guid, ILogger<PerfService> logger);
    }
}