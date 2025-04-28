using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;
using SalesInfrastructure.Data;
using SalesInfrastructure.Data.Repository;

namespace SalesInfrastructure.Configuration;

public static class DataBaseDependencyInjection
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SalesApiDb"), c => c.MigrationsAssembly("SalesInfrastructure")));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection service)
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
}
