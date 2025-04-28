using Microsoft.EntityFrameworkCore;
using SalesDomain.Entities.Sale;
using SalesDomain.Interfaces.Repository;

namespace SalesInfrastructure.Data.Repository;

public class SaleRepository(SalesDbContext context) : ISaleRepository
{
    public async Task Create(Sale sale, CancellationToken cancellationToken = default)
    {
        await context.Sales.AddAsync(sale, cancellationToken);
    }

    public async Task<List<Sale>> GetAll(CancellationToken cancellationToken = default)
    {
        return await context.Sales.Include(c => c.Items)
                                  .AsNoTrackingWithIdentityResolution()
                                  .ToListAsync(cancellationToken);
    }

    public async Task<Sale?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Sales.Include(c => c.Items)
                                  .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}