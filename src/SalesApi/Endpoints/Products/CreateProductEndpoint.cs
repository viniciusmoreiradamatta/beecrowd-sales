using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApplication.Commands.Products.Create;
using SalesApplication.Requests.Products;

namespace SalesApi.Endpoints.Products;

public class CreateProductEndpoint : IEndpointMapper
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("", HandleAsync)
            .WithName("Product: create")
            .WithDescription("Create a new product")
            .WithSummary("Create a new product");
    }

    private static async Task<IResult> HandleAsync([FromServices] ISender sender,
                                                   [FromBody] CreateProductRequest request, CancellationToken cancellation)
    {
        var result = await sender.Send(new Command(request.Price, request.Description, request.Category, request.Image), cancellation);

        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }
}