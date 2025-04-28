using SalesDomain.Validators.Products;

namespace SalesDomain.Entities.Product;

public class Product : Entity
{
    private Product(decimal price, string description, string category, string image) : base(Guid.NewGuid())
    {
        Price = price;
        Description = description;
        Category = category;
        Image = image;
    }

    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }

    protected override void Validate()
    {
        ClearNotifications();

        var validator = new ProductValidator();

        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Notifications.Add(error.ErrorMessage);
            }
        }
    }

    public static Product Create(decimal price, string description, string category, string image)
    {
        var product = new Product(price, description, category, image);

        product.Validate();

        return product;
    }
}