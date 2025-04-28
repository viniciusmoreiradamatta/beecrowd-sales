using Bogus;
using NSubstitute;
using SalesDomain.Abstractions;
using SalesDomain.Entities.Sale;
using SalesTests.Helpers;

namespace SalesTests.Domain.Entities;

public class SaleTests
{
    private readonly Faker faker = new();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    [Fact]
    public void EnsureAllPropertiesAreSetToAvoidHumanError()
    {        // Arrange
        _dateTimeProvider.UtcNow.Returns(Constants.UtcNow);

        var itemList = new List<SaleItem>();

        for (int i = 0; i < 4; i++)
        {
            var productId = Guid.NewGuid();
            var saleId = Guid.NewGuid();
            var quantity = faker.Random.Number(1, 20);
            var unitPrice = faker.Random.Number(5, 150);
            const bool isCancelled = false;

            itemList.Add(SaleItem.CreateSaleItem(productId, saleId, quantity, unitPrice, isCancelled, Guid.NewGuid()));
        }

        var total = itemList.Sum(c => c.Total);
        var branchId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var id = Guid.NewGuid();

        // Act
        var sut = Sale.CreateSale(_dateTimeProvider, customerId, branchId, itemList, id);

        //Assert
        Assert.Equal(_dateTimeProvider.UtcNow, sut.SaleDate);
        Assert.Equal(customerId, sut.CustomerId);
        Assert.Equal(branchId, sut.BranchId);
        Assert.Equal(itemList.Count, sut.Items.Count);
        Assert.Equal(total, sut.TotalAmount);
    }

    [Fact]
    public void ShouldNot_Create_Sale_When_Items_Were_Empty()
    {
        // Arrange
        var id = Guid.NewGuid();

        _dateTimeProvider.UtcNow.Returns(Constants.UtcNow);

        // Act
        var sut = Sale.CreateSale(_dateTimeProvider, Guid.NewGuid(), Guid.NewGuid(), [], id);

        //Assert
        Assert.False(sut.Valid);
        Assert.NotEmpty(sut.Notifications);
    }
}