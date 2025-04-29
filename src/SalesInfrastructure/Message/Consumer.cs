using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SalesDomain.Events;
using SalesDomain.Extensions;
using SalesDomain.Interfaces.Message;

namespace SalesInfrastructure.Message;

public class Consumer(IModel model, ILogger<Consumer> logger) : IConsumer
{
    public Task Receive<TRequest>(string queueName, Func<TRequest, Task> handler) where TRequest : IDomainEvent
    {
        var consumer = new AsyncEventingBasicConsumer(model);

        model.BasicQos(0, 1, false);

        consumer.Received += async (_, eventArgs) =>
        {
            TRequest item = default;

            try
            {
                var body = eventArgs.Body.ToArray();

                item = body.DeserializeBytes<TRequest>();
            }
            catch (Exception ex)
            {
                model.BasicReject(eventArgs.DeliveryTag, false);

                logger.LogCritical(ex, "Error to parse message {Message}", ex.ToString());

                return;
            }

            try
            {
                await handler.Invoke(item);

                model.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                model.BasicNack(eventArgs.DeliveryTag, false, true);

                logger.LogCritical(ex, "Error while process the message {Message}", ex.ToString());
            }
        };

        model.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }
}