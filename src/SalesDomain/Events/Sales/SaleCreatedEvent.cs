namespace SalesDomain.Events.Sales;

public record SaleCreatedEvent(Guid Id, Guid BranchId, decimal TotalAmount, DateTime OccurredOn) : IDomainEvent;