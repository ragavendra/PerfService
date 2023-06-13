using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public interface IActionRunner<T>
   {
      public int Rate { get; set; }

      public ActionBlock<T> ActionBlock { get; set; }

      public LoadDistribution? LoadDistribution_ { get; set; }

      public T TypeValue { get; set; }

      public IList<T> TypeValues { get; set; }

      public Stopwatch Stopwatch { get; set; }

      public Task<TimeSpan> StartActionsPerSecondAsync(int rate);

      public object CloneObj();
    }

}