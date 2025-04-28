using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SalesDomain.Events;
using SalesDomain.Events.Products;
using System.Reflection;

namespace SalesInfrastructure.Configuration.Message
{
    public static class RabbitMqConfiguration
    {
        private const string applicationName = "sales_api";

        public static void ConfigureRabbitMq(this IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var names = new Dictionary<string, string> { };

            Assembly.GetAssembly(typeof(ProductCreatedEvent))?.GetTypes()
                     .Where(t => typeof(IDomainEvent).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                     .ToList().ForEach(c =>
                     {
                         var nome = c.Name.Replace("Event", "");
                         names.Add(nome, nome);
                     });

            service.ConfigureEventQueues(applicationName, names);
        }

        private static void ConfigureEventQueues(this IServiceProvider provider, string applicationName, Dictionary<string, string> routes)
        {
            var model = provider.GetRequiredService<IModel>();

            model.ExchangeDeclare($"sales_api_events", "direct", true, false,
                                  new Dictionary<string, object>() { { "alternate-exchange", $"sales_api_unrouted_exchange" } });

            ConfigureDeadLetter(model);

            ConfigureRetry(model);

            ConfigureUnrouted(model);

            foreach (KeyValuePair<string, string> item in routes)
            {
                string routingKey = item.Key;
                string functionalName = item.Value;

                model.QueueDeclare($"{applicationName}_{functionalName}_queue", true, false, false, new Dictionary<string, object>() {
                { "x-dead-letter-exchange", $"{applicationName}_retry_exchange" }});
                model.QueueBind($"{applicationName}_{functionalName}_queue", "sales_api_events", routingKey, null);
            }
        }

        private static void ConfigureRetry(IModel model)
        {
            model.ExchangeDeclare($"sales_api_retry_exchange", "fanout", true, false, null);

            model.QueueDeclare($"sales_api_retry_queue", true, false, false, new Dictionary<string, object>() {
                { "x-dead-letter-exchange", $"sales_api_deadletter_exchange" },
                { "x-dead-letter-routing-key", "" }
            });
            model.QueueBind($"sales_api_retry_queue", $"sales_api_retry_exchange", string.Empty, null);
        }

        private static void ConfigureDeadLetter(IModel model)
        {
            model.ExchangeDeclare($"sales_api_deadletter_exchange", "fanout", true, false, null);
            model.QueueDeclare($"sales_api_deadletter_queue", true, false, false, null);
            model.QueueBind($"sales_api_deadletter_queue", $"sales_api_deadletter_exchange", string.Empty, null);
        }

        private static void ConfigureUnrouted(IModel model)
        {
            model.ExchangeDeclare($"sales_api_unrouted_exchange", "fanout", true, false, null);
            model.QueueDeclare($"sales_api_unrouted_queue", true, false, false, null);
            model.QueueBind($"sales_api_unrouted_queue", $"sales_api_unrouted_exchange", string.Empty, null);
        }
    }
}