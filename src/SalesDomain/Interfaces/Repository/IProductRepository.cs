using SalesDomain.Entities.Product;

namespace SalesDomain.Interfaces.Repository;

public interface IProductRepository
{
    Task Create(Product product, CancellationToken cancellationToken = default);

    Task<List<Product>> GetAll(CancellationToken cancellationToken = default);
}