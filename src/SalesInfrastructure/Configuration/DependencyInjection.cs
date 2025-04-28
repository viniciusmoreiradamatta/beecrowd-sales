using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;
using SalesInfrastructure.Data;
using SalesInfrastructure.Data.Repository;
using SalesInfrastructure.Message;

namespace SalesInfrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration)
                .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SalesApiDb"), c => c.MigrationsAssembly("SalesInfrastructure")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddScoped<ISaleRepository, SaleRepository>();
        service.AddScoped<IProductRepository, ProductRepository>();

        return service;
    }

    public static async Task ApplyMigration(this IServiceProvider service)
    {
        using var scope = service.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

        await db.Database.MigrateAsync();
    }

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