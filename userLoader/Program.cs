using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Company.Consumers;
// using userLoader;

namespace userLoader
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(service =>
                    {
                        service.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        service.SetInMemorySagaRepositoryProvider();

/*
                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);*/
                        service.AddConsumer<ConsumerConsumer>();

                        service.UsingRabbitMq(configure: (hostContext, conf) =>
                        {
                            conf.Host("localhost", "/", hos =>
                            {
                                hos.Username("guest");
                                hos.Password("guest");
                            }
                            );

                            conf.ConfigureEndpoints(hostContext);
                        });
                    });

                     // services.AddHostedService<Producer>();
                });
    }
}
