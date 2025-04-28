using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Configuration;


using System.Reflection;
using System;
using Parking.Domain.Settings;

namespace Parking.Domain.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransittWithRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(configure => 
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());


                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    configurator.UseMessageRetry(retryConfigurator => 
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            //services.AddMassTransitHostedService();

            return services;
    }
}