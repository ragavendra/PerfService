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

        public ConsumerConsumer(ILogger<UserMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserMessage> context)
        {
            _logger.LogInformation("Hello {name}", context.Message.Title);
            return Task.CompletedTask;
        }
    }
}