using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Network
{
    public class Grpc : IGrpc
    {
        public Guid Guid = Guid.NewGuid();

        public string SampleGrpcMethod()
        {
            return "From sample grpc method";
        }
        
    }
}