using SalesDomain.Entities.Product;

namespace SalesTests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Ensure_All_Properties_Are_Set_To_Avoid_Human_Error()
    {
        // Arrange
        var price = 100;
        var description = "Test Product";
        var category = "Test Category";
        var image = "Test Image";

        // Act
        var sut = Product.Create(price, description, category, image);

        // Assert
        Assert.Equal(price, sut.Price);
        Assert.Equal(description, sut.Description);
        Assert.Equal(category, sut.Category);
        Assert.Equal(image, sut.Image);
        Assert.False(sut.Id == Guid.Empty);
        Assert.True(sut.Valid);
        Assert.Empty(sut.Notifications);
    }

    [Fact]
    public void ShoulNot_Create_Product_With_Zero_Price()
    {
        // Arrange
        var price = 0;
        var description = "Test Product";
        var category = "Test Category";
        var image = "Test Image";

        // Act
        var sut = Product.Create(price, description, category, image);

        //Assert
        Assert.False(sut.Valid);
        Assert.NotEmpty(sut.Notifications);
    }
}