using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
    public class Sample : ISampleScoped, ISampleSingleton, ISampleTransient
    {
        public ILogger<Sample> _logger;

        public Guid Id = Guid.NewGuid();

        public Sample(ILogger<Sample> logger){
            _logger = logger;
        }
        
    }
}