using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Configuration;
using SalesApi.Services;
using SalesApplication.Commands.Sales.GetAll;
using SalesDomain.Abstractions.Response;

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

    private static async Task<IResult> HandleAsync([FromServices] ISender sender, [FromServices] ICacheService cache, CancellationToken cancellationToken)
    {
        var response = await cache.GetAsync<Result<IEnumerable<Response>>?>(Constants.SalesCacheKey, cancellationToken);

        if (!response?.Data?.Any() ?? true)
        {
            response = await sender.Send(new Command(), cancellationToken);

            if (response?.Data?.Any() == true)
            {
                await cache.SetAsync(Constants.SalesCacheKey, response, cancellationToken);
            }
        }

        return Results.Ok(response);
    }
}