using SalesDomain.Abstractions;

namespace SalesDomain.Entities.Sale.Specifications.TaxSpecifications;

public class SpecialTaxSpecification : ISpecification<SaleItem>
{
    public bool IsSatisfiedBy(SaleItem item) => item.Quantity is >= 10;
}
