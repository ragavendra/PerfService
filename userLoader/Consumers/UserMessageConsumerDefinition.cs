namespace Company.Consumers
{
    using MassTransit;

    public class UserMessageConsumerDefinition :
        ConsumerDefinition<UserMessageConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserMessageConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}