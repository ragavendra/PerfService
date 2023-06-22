namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class UserMessageConsumer :
        IConsumer<UserMessage>
    {

        readonly ILogger<UserMessageConsumer> _logger;

        public UserMessageConsumer(ILogger<UserMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserMessage> context)
        {
            _logger.LogInformation("Hello {name}", context.Message.Title);
            Task.Delay(1020);
            return Task.CompletedTask;
        }
    }
}