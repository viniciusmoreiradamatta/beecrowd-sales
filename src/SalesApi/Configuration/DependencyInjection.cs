using SalesApi.Workers;
using SalesApplication.Configuration;
using SalesInfrastructure.Configuration;
using SalesInfrastructure.Configuration.Message;

namespace SalesApi.Configuration;

public static class DependencyInjection
{
    public static async Task AddSalesDependencies(this WebApplicationBuilder app)
    {
        app.Services.AddRabbitMQ()
                    .AddMessaging()
                    .AddApplication()
                    .AddInfrastructure(app.Configuration);

        app.Services.AddHostedService<SaleCreatedWorker>();
        app.Services.AddHostedService<SaleCanceledWorker>();
        app.Services.AddHostedService<ProductCreatedWorker>();

        var provider = app.Services.BuildServiceProvider();

        provider.ConfigureRabbitMq();

        await provider.ApplyMigration();
    }
}