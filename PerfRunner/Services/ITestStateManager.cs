using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public interface ITestStateManager
   {
      //return the first test
      public TestRequest? GetTest(string guid);

      public bool AddTest(TestRequest testRequest);

      public bool RemoveTest(string guid);

   }
}