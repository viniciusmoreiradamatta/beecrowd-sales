namespace SalesApplication.Commands.Products.Create;

public record Response(Guid Id, decimal Price, string Description, string Category, string Image);