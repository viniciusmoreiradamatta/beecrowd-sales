using NSubstitute;
using SalesApplication.Commands.Products.GetAll;
using SalesDomain.Interfaces.Repository;

namespace SalesTests.Application.Commands.Products.GetAll;

public class HandlerTests
{
    private readonly Handler _sut;
    private readonly IProductRepository _productRepository;

    public HandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _sut = new Handler(_productRepository);
    }

    [Fact]
    public async Task Handle_WithExistingProducts_ShouldReturnProductList()
    {
        // Arrange
        var testProducts = Enumerable.Range(0, 3).Select(c => ProductFaker.CreateValidProduct()).ToList();

        _productRepository.GetAll(Arg.Any<CancellationToken>()).Returns(testProducts);

        // Act
        var result = await _sut.Handle(new Command(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Data.Count());

        var firstProduct = result.Data.First();
        Assert.Equal(testProducts[0].Id, firstProduct.Id);
        Assert.Equal(testProducts[0].Price, firstProduct.Price);
        Assert.Equal(testProducts[0].Description, firstProduct.Description);
        Assert.Equal(testProducts[0].Category, firstProduct.Category);
        Assert.Equal(testProducts[0].Image, firstProduct.Image);

        await _productRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        _productRepository.GetAll(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await _sut.Handle(new Command(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
        await _productRepository.Received(1).GetAll(Arg.Any<CancellationToken>());
    }
}