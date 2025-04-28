using Microsoft.Extensions.Caching.Distributed;
using SalesApi.Configuration;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class SaleCreatedWorker(IServiceProvider serviceProvider, IDistributedCache cache, ILogger<SaleCreatedWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_SaleCreated_queue", async (SaleCreatedEvent @event) =>
            {
                logger.LogInformation("Event received {EventId} - {EventOccurredOn} - {EventBranchId} - {EventTotalAmount}",
                                                      @event.Id, @event.OccurredOn, @event.BranchId, @event.TotalAmount);

                await cache.RemoveAsync(Constants.SalesCacheKey, stoppingToken);
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}