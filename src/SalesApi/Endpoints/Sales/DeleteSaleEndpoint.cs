using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Commands.Sales.Delete;

namespace SalesApi.Endpoints.Sales;

public class DeleteSaleEndpoint : IEndpointMapper
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", HandleAsync)
           .WithName("Sales: Delete")
           .WithDescription("Delete a sale")
           .WithSummary("Delete a sale");
    }

    private static async Task<IResult> HandleAsync([FromServices] ISender sender, Guid id, CancellationToken cancellationToken)
    {
        var command = new Command(id);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }
}