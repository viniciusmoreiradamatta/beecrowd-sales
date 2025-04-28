using Bogus;
using SalesApplication.Commands.Products.Create;
using SalesDomain.Entities.Product;

namespace SalesTests.Application.Commands.Products;

public static class ProductFaker
{
    public static Command CreateValidCommand()
    {
        var faker = new Faker();

        return new Command(faker.Random.Decimal(1, 250), faker.Random.String(), faker.Random.String(), faker.Random.String());
    }

    public static Command CreateInValidCommand()
    {
        var faker = new Faker();

        return new Command(faker.Random.Decimal(-10, 0), faker.Random.String(), faker.Random.String(), faker.Random.String());
    }

    public static Product CreateValidProduct()
    {
        var faker = new Faker();

        return Product.Create(faker.Random.Decimal(1, 250), faker.Random.String(), faker.Random.String(), faker.Random.String());
    }
}