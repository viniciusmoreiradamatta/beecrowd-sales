using MediatR;
using SalesDomain.Abstractions.Response;

namespace SalesApplication.Commands.Sales.Delete;

public record Command(Guid Id) : IRequest<Result>;