using MediatR;
using Microsoft.Extensions.Logging;
using SalesDomain.Abstractions;
using SalesDomain.Abstractions.Response;
using SalesDomain.Entities.Sale;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesApplication.Commands.Sales.Create;

public class Handler(ISaleRepository saleRepotory, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider,
                     IProducer producer, ILogger<Handler> logger) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        try
        {
            var saleId = Guid.NewGuid();
            var items = request.Items.Select(item =>
                SaleItem.CreateSaleItem(item.ProductId, saleId, item.Quantity, item.UnitPrice, false)).ToList();

            var sale = Sale.CreateSale(dateTimeProvider, request.CustomerId, request.BranchId, items, saleId, request.SaleNumber);

            if (!sale.Valid)
                return Result<Response>.CreateErrorResponse($"The follow errors occour during validation sale: {string.Join('\n', sale.Notifications)}");

            await saleRepotory.Create(sale, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            await producer.Notify(new SaleCreatedEvent(sale.Id, sale.BranchId, sale.TotalAmount, dateTimeProvider.UtcNow));

            return Result<Response>.CreateSuccessResponse(Response.MapToResponse(sale), "Sale succesfuly created");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return Result<Response>.CreateErrorResponse("An unexpectd error occour while creating sale. Please try again later!");
        }
    }
}