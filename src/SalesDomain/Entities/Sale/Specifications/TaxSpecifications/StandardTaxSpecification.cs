using SalesDomain.Abstractions;

namespace SalesDomain.Entities.Sale.Specifications.TaxSpecifications;

public class StandardTaxSpecification : ISpecification<SaleItem>
{
    public bool IsSatisfiedBy(SaleItem item) => item.Quantity is >= 4 and < 10;
}
