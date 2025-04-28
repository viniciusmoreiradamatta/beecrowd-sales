using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;

namespace SalesApi.Workers;

public class SaleCanceledWorker(IServiceProvider serviceProvider, ILogger<SaleCanceledWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var consumer = provider.GetRequiredService<IConsumer>();

            await consumer.Receive("sales_api_SaleCancelled_queue", (SaleCancelledEvent @event) =>
            {
                Console.WriteLine($"Event received {@event.Id} -{@event.OccurredOn} - {@event.Id}");
                return Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
        }
    }
}