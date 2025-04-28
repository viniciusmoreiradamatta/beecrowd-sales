namespace SalesDomain.Events.Products;

public record ProductCreatedEvent(DateTime OccurredOn, Guid Id, decimal Price,
                                  string Category, string Description, string Image) : IDomainEvent;