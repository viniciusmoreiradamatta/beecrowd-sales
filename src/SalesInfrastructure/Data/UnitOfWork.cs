using SalesDomain.Interfaces.UnitOfWork;

namespace SalesInfrastructure.Data;

public class UnitOfWork(SalesDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) => await context.SaveChangesAsync(cancellationToken);
}