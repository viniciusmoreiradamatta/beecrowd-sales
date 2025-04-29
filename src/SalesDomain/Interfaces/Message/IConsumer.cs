using SalesDomain.Events;

namespace SalesDomain.Interfaces.Message;

public interface IConsumer
{
    Task Receive<TRequest>(string queueName, Func<TRequest, Task> handler) where TRequest : IDomainEvent;
}