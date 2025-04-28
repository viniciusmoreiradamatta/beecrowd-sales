using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Configuration;
using SalesApi.Services;
using SalesApplication.Commands.Products.GetAll;
using SalesDomain.Abstractions.Response;

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

    private static async Task<IResult> HandleAsync([FromServices] ISender sender, [FromServices] ICacheService cache, CancellationToken cancellationToken)
    {
        var response = await cache.GetAsync<Result<IEnumerable<Response>>?>(Constants.ProductsCacheKey, cancellationToken);

        if (!response?.Data?.Any() ?? true)
        {
            response = await sender.Send(new Command(), cancellationToken);

            if (response?.Data?.Any() == true)
            {
                await cache.SetAsync(Constants.ProductsCacheKey, response, cancellationToken);
            }
        }

        var result = await sender.Send(new Command(), cancellationToken);

        return Results.Ok(result);
    }
}