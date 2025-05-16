using SalesDomain.Abstractions;

namespace SalesDomain.Entities.Sale.Specifications.TaxSpecifications;

public class FreeTaxSpecification : ISpecification<SaleItem>
{
    public bool IsSatisfiedBy(SaleItem item) => item.Quantity is > 0 and <= 3;
}
