using MediatR;
using SalesDomain.Abstractions.Response;

namespace SalesApplication.Commands.Sales.GetAll;

public record Command() : IRequest<Result<IEnumerable<Response>>>;