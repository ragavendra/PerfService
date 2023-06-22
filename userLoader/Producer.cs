using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using Contracts;

namespace userLoader
{
   public class Producer : BackgroundService
   {
      readonly ILogger<Producer> _logger;
      readonly IBus _bus;
      long _accountIndex;
      long _totalUsers;

      CancellationToken cancellationToken;

      public Producer(IBus bus, ILogger<Producer> logger)
      {
         _bus = bus;
         _logger = logger;
         _accountIndex = UserFormatInfo.UserStartIndex;
         _totalUsers = UserFormatInfo.TotalUsers;
      }

      protected override async Task ExecuteAsync(CancellationToken cts)
      {
        cancellationToken = cts;
         while (!cts.IsCancellationRequested)
         {
            LoadUsersAsync();
            // await _bus.Publish(new UserMessage() { Title = $"Hi there {DateTime.Now.ToString()}!" }, cts);

            // await Task.Delay(1000, cts);
            await Task.Delay(1000, cancellationToken);
         }
      }

      // load users to ready state
      private async Task LoadUsersAsync()
      {
         // var totalUsers = UserFormatInfo.TotalUsers;
         // var accountIndex = UserFormatInfo.UserStartIndex;
         while ((Interlocked.Decrement(ref _totalUsers) >= 0) && !cancellationToken.IsCancellationRequested)
         {
            var user = new User(string.Format(UserFormatInfo.UserAccountFormat, Interlocked.Increment(ref _accountIndex)), UserState.Ready);
            _logger.LogInformation("Loading {guid}", user.Email);
            await CheckInUserAsync(user);
         }
      }

      public async Task<bool> CheckInUserAsync(User user)
      {
         /* _bus.Publish<User>(message =>
         {
            // user
            // message.Durable = false;
            // message.AutoDelete = true;
            // message.ExchangeType = "fanout";
         });*/

         await _bus.Publish(user);
         Thread.Sleep(1000);

         return true;
      }

   }
}