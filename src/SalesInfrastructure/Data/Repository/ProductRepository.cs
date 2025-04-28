using Microsoft.EntityFrameworkCore;
using SalesDomain.Entities.Product;
using SalesDomain.Interfaces.Repository;

namespace SalesInfrastructure.Data.Repository;

public class ProductRepository(SalesDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetAll(CancellationToken cancellationToken = default)
    {
        return await context.Products.AsNoTracking()
                                     .ToListAsync(cancellationToken);
    }

    public async Task Create(Product product, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
    }
}