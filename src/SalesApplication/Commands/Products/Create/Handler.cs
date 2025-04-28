using MediatR;
using Microsoft.Extensions.Logging;
using SalesDomain.Abstractions;
using SalesDomain.Abstractions.Response;
using SalesDomain.Entities.Product;
using SalesDomain.Events.Products;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesApplication.Commands.Products.Create;

public class Handler(IProductRepository productRepository, IUnitOfWork unitOfWork, IProducer producer,
                     IDateTimeProvider provider, ILogger<Handler> logger) : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
    {
        try
        {
            var product = Product.Create(command.Price, command.Description, command.Category, command.Image);

            if (!product.Valid)
            {
                return Result<Response>.CreateErrorResponse(string.Join('\n', product.Notifications));
            }

            await productRepository.Create(product, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            await producer.Notify(new ProductCreatedEvent(provider.UtcNow, product.Id, product.Price, product.Category, product.Description, product.Image));

            var response = new Response(product.Id, product.Price, product.Description, product.Category, product.Image);

            return Result<Response>.CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            return Result<Response>.CreateErrorResponse("An unexpectd error occour while creating sale. Please try again later!");
        }
    }
}