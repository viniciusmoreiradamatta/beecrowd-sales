using MediatR;
using SalesDomain.Abstractions.Response;
using SalesDomain.Interfaces.Repository;

namespace SalesApplication.Commands.Sales.GetAll;

public class Handler(ISaleRepository saleRepotory) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Command request, CancellationToken cancellationToken)
    {
        var seles = await saleRepotory.GetAll(cancellationToken);

        var responseList = seles.Count > 0 ? seles.Select(c => new Response
        {
            BranchId = c.BranchId,
            Cancelled = c.Cancelled,
            CustomerId = c.CustomerId,
            Date = c.SaleDate,
            Id = c.Id,
            SaleNumber = c.SaleNumber,
            TotalAmount = c.TotalAmount,
            Items = c.Items.Select(cc => new Response.Item
            {
                Id = cc.Id,
                IsCancelled = c.Cancelled,
                ProductId = cc.ProductId,
                Quantity = cc.Quantity,
                SaleId = cc.SaleId,
                Total = cc.Total,
                UnitPrice = cc.UnitPrice,
                ValueMonetaryTaxApplied = cc.ValueMonetaryTaxApplied
            }).ToList()
        }) : [];

        return Result<IEnumerable<Response>>.CreateSuccessResponse(responseList);
    }
}