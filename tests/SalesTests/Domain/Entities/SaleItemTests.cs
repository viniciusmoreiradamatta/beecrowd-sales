using SalesDomain.Entities.Sale;

namespace SalesTests.Domain.Entities;

public class SaleItemTests
{
    [Fact]
    public void Ensure_All_Properties_Are_Set_To_Avoid_Human_Error()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var saleId = Guid.NewGuid();
        var quantity = 5;
        var unitPrice = 100;
        const bool isCancelled = false;

        // Act
        var sut = SaleItem.CreateSaleItem(productId, saleId, quantity, unitPrice, isCancelled, Guid.NewGuid());

        // Assert
        Assert.Equal(productId, sut.ProductId);
        Assert.Equal(saleId, sut.SaleId);
        Assert.Equal(unitPrice, sut.UnitPrice);
        Assert.Equal(quantity, sut.Quantity);
        Assert.Equal(isCancelled, sut.IsCancelled);
    }

    [Fact]
    public void Should_Create_SaleItem_And_Calc_StandartTaxe_Correctly()
    {
        // Arrange
        var quantity = 5;
        var saleId = Guid.NewGuid();
        const int unitPrice = 100;

        // Act
        var sut = SaleItem.CreateSaleItem(Guid.NewGuid(), saleId, quantity, unitPrice, false, Guid.NewGuid());

        // Assert
        Assert.Equal(quantity, sut.Quantity);
        Assert.Equal(550, sut.Total);
        Assert.Equal(50, sut.ValueMonetaryTaxApplied);
    }

    [Fact]
    public void Should_Create_SaleItem_And_Calc_SpecialTaxe_Correctly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var saleId = Guid.NewGuid();
        var quantity = 15;
        var unitPrice = 50;

        // Act
        var sut = SaleItem.CreateSaleItem(productId, saleId, quantity, unitPrice, false, Guid.NewGuid());

        // Assert
        Assert.Equal(quantity, sut.Quantity);
        Assert.Equal(900, sut.Total);
        Assert.Equal(150, sut.ValueMonetaryTaxApplied);
    }

    [Fact]
    public void ShouldNot_Create_SaleItem_When_MaxQuantity_Execendent()
    {
        // Arrange Act
        var sut = SaleItem.CreateSaleItem(Guid.NewGuid(), Guid.NewGuid(), 21, 100, false, Guid.NewGuid());

        // Assert
        Assert.False(sut.Valid);
    }
}