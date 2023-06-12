using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
    public interface IActionRunner<T>
    {
        
    }

    public enum LoadDistribution
    {
        Even,
        Uneven
    }
}