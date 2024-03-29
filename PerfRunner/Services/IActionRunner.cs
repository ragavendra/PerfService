using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks.Dataflow;
using PerfRunner.V1;

namespace PerfRunner.Services
{
    public interface IActionRunner<T>
    {
        public Guid Guid { get; set; }

        public Guid TestGuid { get; set; }

        public Counter<int>? RunCounter { get; set; }

        public Histogram<double>? RunHistogram { get; set; }

        public ActionBlock<T> ActionBlock { get; set; }

        public T TypeValue { get; set; }

        public ActionOption ActionOption { get; set; }

        public Task<bool> StartActionsPerSecondAsync(int rate);

        public object CloneObj();
    }
}