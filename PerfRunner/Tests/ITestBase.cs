using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Tests
{
    public interface ITestBase
    {
      public HttpClient _httpClient { get; set; }

      public void RunTest(Guid guid);
    }
}