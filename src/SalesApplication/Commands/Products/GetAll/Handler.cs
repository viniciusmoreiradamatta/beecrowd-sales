using MediatR;
using SalesDomain.Abstractions.Response;
using SalesDomain.Interfaces.Repository;

namespace SalesApplication.Commands.Products.GetAll;

public class Handler(IProductRepository productRepository) : IRequestHandler<Command, Result<IEnumerable<Response>>>
{
    public async Task<Result<IEnumerable<Response>>> Handle(Command request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAll(cancellationToken);

        var responseList = products.Count > 0 ? products.Select(c => new Response
        {
            Category = c.Category,
            Id = c.Id,
            Description = c.Description,
            Image = c.Image,
            Price = c.Price,
        }) : [];

        return Result<IEnumerable<Response>>.CreateSuccessResponse(responseList);
    }
}