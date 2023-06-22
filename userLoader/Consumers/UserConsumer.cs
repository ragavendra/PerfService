namespace Company.Consumers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class UserConsumer :
        IConsumer<User>
    {

        readonly ILogger<UserConsumer> _logger;

        public UserConsumer(ILogger<UserConsumer> logger)
        {
            _logger = logger;
            Thread.Sleep(1020);
        }

        // runs parallely and consumes almost all in no time.
        public Task Consume(ConsumeContext<User> context)
        {
            _logger.LogInformation("Consuming {name}", context.Message.Email);
            Task.Delay(1020);
            return Task.CompletedTask;
        }
    }
}