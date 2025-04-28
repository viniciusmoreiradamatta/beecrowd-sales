using Microsoft.Extensions.Configuration;
using SalesApi.Services;
using SalesApi.Workers;
using SalesApplication.Configuration;
using SalesInfrastructure.Configuration;
using SalesInfrastructure.Configuration.Message;

namespace SalesApi.Configuration;

public static class DependencyInjection
{
    public static async Task AddSalesDependencies(this WebApplicationBuilder app)
    {
        app.Services.AddCache(app.Configuration)
                    .AddApplication()
                    .AddDbContext(app.Configuration)
                    .AddRepositories()
                    .AddRabbitMQ()
                    .AddMessaging();

        var provider = app.Services.BuildServiceProvider();

        provider.ConfigureRabbitMq();

        app.Services.AddHostedService<SaleCreatedWorker>();
        app.Services.AddHostedService<SaleCanceledWorker>();
        app.Services.AddHostedService<ProductCreatedWorker>();

        app.Services.AddScoped<ICacheService, CacheService>();

        await provider.ApplyMigration();
    }

    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            var redisSettings = new RedisCacheSettings();

            configuration.GetSection("Redis").Bind(redisSettings);

            options.Configuration = redisSettings.Configuration;
            options.InstanceName = redisSettings.InstanceName;
        });

        return services;
    }
}