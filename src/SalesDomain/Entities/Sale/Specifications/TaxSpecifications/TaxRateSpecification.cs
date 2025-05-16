using SalesDomain.Abstractions;
using SalesDomain.Exceptions;

namespace SalesDomain.Entities.Sale.Specifications.TaxSpecifications;

public class TaxRateSpecification
{
    private readonly List<(ISpecification<SaleItem> Specification, Func<SaleItem, decimal> TaxRateResolver)> _specifications;

    public TaxRateSpecification()
    {
        _specifications =
        [
            (new StandardTaxSpecification(), item => item.StandartIva),
            (new SpecialTaxSpecification(), item => item.SpecialIva),
            (new FreeTaxSpecification(), _ => 0.0m)
        ];
    }

    public decimal GetTaxRate(SaleItem item)
    {
        foreach (var (specification, taxRateResolver) in _specifications)
        {
            if (specification.IsSatisfiedBy(item))
            {
                return taxRateResolver(item);
            }
        }

        throw new NoTaxRateFoundException(item);
    }
}