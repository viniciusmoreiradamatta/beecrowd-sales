namespace SalesApi.Endpoints;

public interface IEndpointMapper
{
    static abstract void Map(IEndpointRouteBuilder app);
}