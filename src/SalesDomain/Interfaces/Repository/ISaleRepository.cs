using SalesDomain.Entities.Sale;

namespace SalesDomain.Interfaces.Repository;

public interface ISaleRepository
{
    Task Create(Sale sale, CancellationToken cancellationToken = default);

    Task<List<Sale>> GetAll(CancellationToken cancellationToken = default);

    Task<Sale?> GetById(Guid id, CancellationToken cancellationToken = default);
}