using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Network
{
    public interface IGrpc : IProtocol
    {
        public string SampleGrpcMethod();
        
    }
}