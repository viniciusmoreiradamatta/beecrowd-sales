namespace SalesApplication.Requests.Products;

public record CreateProductRequest(string Image, decimal Price, string Category, string Description);