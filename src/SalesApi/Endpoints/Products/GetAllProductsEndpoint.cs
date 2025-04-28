using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Commands.Products.GetAll;

namespace SalesApi.Endpoints.Products;

public class GetAllProductsEndpoint : IEndpointMapper
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("", HandleAsync)
           .WithName("Product: get all products")
           .WithDescription("Returns all products")
           .WithSummary("Returns all products");
    }

    private static async Task<IResult> HandleAsync([FromServices] ISender sender, CancellationToken cancellation)
    {
        var result = await sender.Send(new Command(), cancellation);

        return Results.Ok(result);
    }
}