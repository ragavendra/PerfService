using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.Services;

namespace PerfRunner.Tests
{
    public interface ITestBase
    {
      public HttpClient _httpClient { get; set; }

      public void RunTest(Guid guid, ILogger<PerfService> logger);
    }
}