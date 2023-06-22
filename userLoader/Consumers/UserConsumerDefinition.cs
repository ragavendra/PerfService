namespace Company.Consumers
{
    using MassTransit;

    public class UserConsumerDefinition :
        ConsumerDefinition<UserConsumer>
    {
        public UserConsumerDefinition()
        {
            ConcurrentMessageLimit = 2;

        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}