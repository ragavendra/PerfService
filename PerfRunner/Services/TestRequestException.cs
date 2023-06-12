using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
    public class TestRequestException : ApplicationException
    {
        public TestRequestException(string message) : base(message)
        {}

        public TestRequestException(string message, Exception innerException) : base(message, innerException)
        {}   
    }
}