using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfRunner.Services
{
   public class Greeter : BackgroundService
   {
      private readonly ILogger<BackgroundService> _logger;

      private Guid _guid;// = Guid.NewGuid();

      public Greeter(ILogger<Greeter> logger)
      {
         _logger = logger;
         _guid = Guid.NewGuid();
      }

      protected override async Task ExecuteAsync(CancellationToken stoppingToken)
      {
         while (!stoppingToken.IsCancellationRequested)
         {
            _logger.LogInformation($"Helo from the {GetType().Name} service - {_guid} .");

            await Task.Delay(1000, stoppingToken);
         }

         // return default;
         // throw new NotImplementedException();
      }
   }
}