using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SalesDomain.Events;
using SalesDomain.Extensions;
using SalesDomain.Interfaces.Message;

namespace SalesInfrastructure.Message;

public class Producer(IModel model, IConfiguration configuration, ILogger<Producer> logger) : IProducer
{
    public async Task Notify<TEvent>(TEvent _event) where TEvent : IDomainEvent
    {
        var basicProperties = MakeBasicProperties(_event.Id);

        model.ConfirmSelect();

        var appName = configuration.GetSection("Config:appname").Value;

        var eventName = _event.GetType().Name.Replace("Event", "");

        var destination = $"sales_api_events";

        var body = _event.SerializeBytes();

        await SendMessage(body, basicProperties, destination, eventName);
    }

    private IBasicProperties MakeBasicProperties(Guid id)
    {
        var basicProperties = model.CreateBasicProperties();

        basicProperties.MessageId = id != Guid.Empty ? id.ToString("D")
                                                     : Guid.NewGuid().ToString("D");

        basicProperties.DeliveryMode = 2;

        basicProperties.Headers = new Dictionary<string, object>()
            {
                {"content-type","application/json" },
                {"applicationName","appName" },
            };

        return basicProperties;
    }

    private Task SendMessage(byte[] body, IBasicProperties basicProperties, string destination, string key)
    {
        try
        {
            model.BasicPublish(exchange: destination,
                                routingKey: key,
                                basicProperties: basicProperties,
                                body: body);

            model.WaitForConfirmsOrDie(TimeSpan.FromMinutes(1));
        }
        catch (Exception ee)
        {
            logger.LogError(ee, "Error to send message {Message}", ee.ToString());
        }

        return Task.CompletedTask;
    }
}