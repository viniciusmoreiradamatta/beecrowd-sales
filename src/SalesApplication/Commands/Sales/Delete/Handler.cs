using MediatR;
using Microsoft.Extensions.Logging;
using SalesDomain.Abstractions.Response;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesApplication.Commands.Sales.Delete;

public class Handler(ISaleRepository saleRepository,
                     IUnitOfWork unitOfWork,
                     ILogger<Handler> logger) : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
    {
        try
        {
            var sale = await saleRepository.GetById(command.Id, cancellationToken);

            if (sale is null)
                return Result.CreateErrorResponse("Sell not found");

            sale.Cancel();

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.CreateSuccessResponse("Sell Cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());

            return Result.CreateErrorResponse("An unexpectd error occour while canceling sell. Please try again later!");
        }
    }
}