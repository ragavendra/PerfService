using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Network
{
    public class Http : IHttp
    {
        public Guid Guid = Guid.NewGuid();

        public string SampleHttpMethod()
        {
            return "From sample http method";
        }
        
    }
}