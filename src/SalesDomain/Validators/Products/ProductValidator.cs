using FluentValidation;
using SalesDomain.Entities.Product;

namespace SalesDomain.Validators.Products;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(c => c.Price).GreaterThan(0)
          .WithMessage("Price must be greater than 0.");

        RuleFor(c => c.Description).NotEmpty().MaximumLength(80);

        RuleFor(c => c.Category).NotEmpty().MaximumLength(80);

        RuleFor(c => c.Image).NotEmpty().MaximumLength(255);
    }
}