using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Commands.Sales.Create;
using SalesApplication.Requests.Sales;

namespace SalesApi.Endpoints.Sales;

public class CreateSaleEndpoint : IEndpointMapper
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("", HandleAsync)
           .WithName("Sales: create a sale")
           .WithDescription("Create a sale")
           .WithSummary("Create a sale");
    }

    private static async Task<IResult> HandleAsync([FromServices] ISender sender,
                                                   [FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = new Command()
        {
            BranchId = request.BranchId,
            CustomerId = request.CustomerId,
            SaleDate = request.Date,
            SaleNumber = request.SaleNumber,
            Items = request.Items.Select(c => new Command.Item { ProductId = c.ProductId, Quantity = c.Quantity, UnitPrice = c.UnitPrice }).ToList()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Data) : Results.BadRequest(result.Message);
    }
}