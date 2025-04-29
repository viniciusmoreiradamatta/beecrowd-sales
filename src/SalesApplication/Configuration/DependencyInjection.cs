using Microsoft.Extensions.DependencyInjection;
using SalesDomain.Abstractions;

namespace SalesApplication.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}