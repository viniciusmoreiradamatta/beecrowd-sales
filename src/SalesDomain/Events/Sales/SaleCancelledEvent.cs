namespace SalesDomain.Events.Sales;

public record SaleCancelledEvent(DateTime OccurredOn, Guid Id) : IDomainEvent;