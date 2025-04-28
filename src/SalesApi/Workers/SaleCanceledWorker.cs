using Microsoft.Extensions.Caching.Distributed;
using SalesApi.Configuration;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class SaleCanceledWorker(IServiceProvider serviceProvider, IDistributedCache cache, ILogger<SaleCanceledWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_SaleCancelled_queue", async (SaleCancelledEvent @event) =>
            {
                logger.LogInformation("Event received {EventId} - {EventOccurredOn}", @event.Id, @event.OccurredOn);

                await cache.RemoveAsync(Constants.SalesCacheKey, stoppingToken);
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}