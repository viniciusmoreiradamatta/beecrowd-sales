using SalesDomain.Events;

namespace SalesDomain.Interfaces.Message;

public interface IProducer
{
    Task Notify<TEvent>(TEvent _event) where TEvent : IDomainEvent;
}