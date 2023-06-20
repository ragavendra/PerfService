namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class ConsumerConsumer :
        IConsumer<UserMessage>
    {

        readonly ILogger<ConsumerConsumer> _logger;

        public ConsumerConsumer(ILogger<ConsumerConsumer> logger)
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