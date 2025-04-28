using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class SaleCreatedWorker(IServiceProvider serviceProvider, ILogger<SaleCreatedWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_SaleCreated_queue", (SaleCreatedEvent @event) =>
            {
                Console.WriteLine($"Event received {@event.Id} - {@event.OccurredOn} - {@event.BranchId} - {@event.TotalAmount}");
                return Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}