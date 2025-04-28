using FluentValidation;
using SalesDomain.Entities.Sale;

namespace SalesDomain.Validators.Sales;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).LessThan(20);

        RuleFor(x => x.Total).GreaterThan(0);

        RuleFor(c => c.ProductId).NotEmpty();

        RuleFor(c => c.SaleId).NotEmpty();

        RuleFor(c => c.UnitPrice).GreaterThan(0);
    }
}