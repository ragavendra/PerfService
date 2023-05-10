using System.Collections.Concurrent;
using Grpc.Core;
using PerfRunner.V1;

namespace PerfRunner.Tests
{
   // Static class to maintain or manage test(s).
   public class Login : TestBase
   {
      public Guid Guid = Guid.NewGuid();

/*
      private readonly ILogger<Login> _logger;

      public Login(ILogger<Login> logger)
      {
         _logger = logger;
      }*/

      public override void RunTest()
      {
         _logger?.LogInformation($"Running {nameof(this.GetType)} now.");
         // Console.WriteLine($"Running {nameof(this.GetType)} now.");
      }
   }
}