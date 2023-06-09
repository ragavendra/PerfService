using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
    public interface ISample
    {
        
    }

    public interface ISampleTransient : ISample
    {
        
    }

    public interface ISampleScoped : ISample
    {
        
    }

    public interface ISampleSingleton : ISample
    {
        
    }
}