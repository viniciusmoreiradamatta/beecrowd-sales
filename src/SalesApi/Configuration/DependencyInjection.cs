using SalesApplication.Configuration;
using SalesInfrastructure.Configuration;

namespace SalesApi.Configuration;

public static class DependencyInjection
{
    public static async Task AddSalesDependencies(this WebApplicationBuilder app)
    {
        app.Services.AddApplication()
                    .AddInfrastructure(app.Configuration);

        var provider = app.Services.BuildServiceProvider();

        await provider.ApplyMigration();
    }
}