using Bogus;
using NSubstitute;
using SalesApplication.Commands.Sales.Create;
using SalesDomain.Abstractions;
using SalesDomain.Entities.Sale;

namespace SalesTests.Application.Commands.Sales
{
    public static class SaleFaker
    {
        public static Command CreateValidRequest()
        {
            var faker = new Faker();

            return new()
            {
                CustomerId = faker.Random.Guid(),
                BranchId = faker.Random.Guid(),
                Items = [new () { ProductId = faker.Random.Guid(),
                                 Quantity = faker.Random.Int(1, 20),
                                 UnitPrice = faker.Random.Decimal(10, 1500)}]
            };
        }

        public static Command CreateInvalidRequest()
        {
            var faker = new Faker();

            return new()
            {
                CustomerId = faker.Random.Guid(),
                BranchId = faker.Random.Guid(),
                Items = [new () { ProductId = faker.Random.Guid(),
                                 Quantity = faker.Random.Int(10, 30),
                                 UnitPrice = faker.Random.Decimal(10, 1500)}]
            };
        }

        public static Sale CreateSale()
        {
            var faker = new Faker();

            return Sale.CreateSale(Substitute.For<IDateTimeProvider>(),
                                   faker.Random.Guid(),
                                   faker.Random.Guid(),
                                   [SaleItem.CreateSaleItem(faker.Random.Guid(),
                                                            faker.Random.Guid(),
                                                            faker.Random.Int(1, 20),
                                                            faker.Random.Decimal(10, 1500),
                                                            false)],
                                    faker.Random.Guid()
            );
        }
    }
}