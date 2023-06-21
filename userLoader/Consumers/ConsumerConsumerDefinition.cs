namespace Company.Consumers
{
    using MassTransit;

    public class ConsumerConsumerDefinition :
        ConsumerDefinition<ConsumerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ConsumerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}