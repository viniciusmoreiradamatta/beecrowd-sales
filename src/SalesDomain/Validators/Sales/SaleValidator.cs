using FluentValidation;
using SalesDomain.Entities.Sale;

namespace SalesDomain.Validators.Sales;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(c => c.Items).NotEmpty()
            .WithMessage("Sale must have at least one item.");

        RuleFor(c => c.BranchId).NotEmpty();

        RuleFor(c => c.CustomerId).NotEmpty();
    }
}