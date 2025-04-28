using MediatR;
using SalesDomain.Abstractions.Response;

namespace SalesApplication.Commands.Products.GetAll;

public record Command : IRequest<Result<IEnumerable<Response>>>;