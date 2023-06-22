namespace PerfRunner.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class UserConsumer :
        IConsumer<User>
    {

        readonly ILogger<UserConsumer> _logger;

        public ConsumerConsumer(ILogger<UserConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<User> context)
        {
            _logger.LogInformation("Consuming {name}", context.Message.Email);
            Task.Delay(1020);
            return Task.CompletedTask;
        }
    }
}