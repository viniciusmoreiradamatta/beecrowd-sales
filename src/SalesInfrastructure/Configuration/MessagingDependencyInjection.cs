using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SalesDomain.Interfaces.Message;
using SalesInfrastructure.Message;

namespace SalesInfrastructure.Configuration;

public static class MessagingDependencyInjection
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection service, string configSection = "rabbitmq")
    {
        service.AddSingleton(provider =>
        {
            ConnectionFactory factory = new();

            provider.GetRequiredService<IConfiguration>().Bind(configSection, factory);

            return factory;
        });

        service.AddSingleton(sp =>
            Policy.Handle<BrokerUnreachableException>().WaitAndRetry(3, retryAttempt =>
            {
                TimeSpan wait = TimeSpan.FromSeconds(30);

                return wait;
            }).Execute(() =>
            {
                System.Diagnostics.Debug.WriteLine("Trying to create a connection with RabbitMQ");

                IConnection connection = sp.GetRequiredService<ConnectionFactory>().CreateConnection();

                return connection;
            }));

        service.AddScoped(sp => sp.GetRequiredService<IConnection>().CreateModel());

        return service;
    }

    public static IServiceCollection AddMessaging(this IServiceCollection service)
    {
        service.AddScoped<IProducer, Producer>();
        service.AddScoped<IConsumer, Consumer>();

        return service;
    }
}