using SalesApi.Endpoints.Products;
using SalesApi.Endpoints.Sales;

namespace SalesApi.Endpoints;

public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("products").WithTags("Products")
                 .MapEndpoint<CreateProductEndpoint>()
                 .MapEndpoint<GetAllProductsEndpoint>();

        endpoints.MapGroup("sales").WithTags("Sales")
                 .MapEndpoint<CreateSaleEndpoint>()
                 .MapEndpoint<DeleteSaleEndpoint>()
                 .MapEndpoint<GetAllSalesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpointMapper
    {
        TEndpoint.Map(app);

        return app;
    }
}
