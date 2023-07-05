using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public interface IActionRunner<T>
   {
      public Guid Guid { get; }

      public Guid TestGuid { get; set; }

      public int Rate { get; set; }

      public Histogram<double> RunCounter { get; set; }

      public ActionBlock<T> ActionBlock { get; set; }

      public LoadDistribution? LoadDistribution_ { get; set; }

      public T TypeValue { get; set; }

      public Task<TimeSpan> StartActionsPerSecondAsync(int rate);

      public object CloneObj();
    }

}