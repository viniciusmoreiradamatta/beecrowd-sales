using SalesDomain.Events.Products;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class ProductCreatedWorker(IServiceProvider serviceProvider, ILogger<ProductCreatedWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_ProductCreated_queue", (ProductCreatedEvent @event) =>
            {
                Console.WriteLine($"Event received {@event.Id} -{@event.OccurredOn} - {@event.Price} - {@event.Description}");
                return Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}