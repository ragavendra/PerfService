using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
    public class SampleTwo
    {
        public ILogger<SampleTwo> _logger;

        public Guid Id = Guid.NewGuid();

        public SampleTwo(ILogger<SampleTwo> logger){
            _logger = logger;
        }
        
    }
}