using MediatR;
using Microsoft.Extensions.Logging;
using SalesDomain.Abstractions;
using SalesDomain.Abstractions.Response;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesApplication.Commands.Sales.Delete;

public class Handler(ISaleRepository saleRepository,
                     IUnitOfWork unitOfWork,
                     IProducer producer,
                     IDateTimeProvider provider,
                     ILogger<Handler> logger) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        var sale = await saleRepository.GetById(command.Id, cancellationToken);

        if (sale is null)
            return Result.CreateErrorResponse("Sell not found", EStatusResponse.NotFound);

        sale.Cancel();

        await unitOfWork.CommitAsync(cancellationToken);

        await producer.Notify(new SaleCancelledEvent(provider.UtcNow, command.Id));

        return Result.CreateSuccessResponse("Sell Cancelled");
    }
}