using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Commands.Sales.GetAll;

namespace SalesApi.Endpoints.Sales;

public class GetAllSalesEndpoint : IEndpointMapper
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("", HandleAsync)
            .WithName("Sales: get all sales")
            .WithDescription("Returns all sales")
            .WithSummary("Returns all sales");
    }

    private static async Task<IResult> HandleAsync([FromServices] ISender sender, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new Command(), cancellationToken);

        return Results.Ok(response);
    }
}