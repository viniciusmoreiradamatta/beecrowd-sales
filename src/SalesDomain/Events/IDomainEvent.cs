namespace SalesDomain.Events;

public interface IDomainEvent
{
    public DateTime OccurredOn { get; }
    public Guid Id { get; }
}