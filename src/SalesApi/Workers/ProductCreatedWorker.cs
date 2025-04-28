using Microsoft.Extensions.Caching.Distributed;
using SalesApi.Configuration;
using SalesDomain.Events.Products;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class ProductCreatedWorker(IServiceProvider serviceProvider, IDistributedCache cache, ILogger<ProductCreatedWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_ProductCreated_queue", async (ProductCreatedEvent @event) =>
            {
                logger.LogInformation("Event received {EventId} - {EventOccurredOn} - {EventPrice} - {EventDescription}",
                                                       @event.Id, @event.OccurredOn, @event.Price, @event.Description);

                await cache.RemoveAsync(Constants.ProductsCacheKey, stoppingToken);
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}