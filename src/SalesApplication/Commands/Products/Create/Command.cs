using MediatR;
using SalesDomain.Abstractions.Response;

namespace SalesApplication.Commands.Products.Create;

public record Command(decimal Price, string Description, string Category, string Image) : IRequest<Result<Response>>;