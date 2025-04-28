using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SalesApplication.Commands.Products.Create;
using SalesDomain.Entities.Product;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesTests.Application.Commands.Products.Create;

public class HandlerTests
{
    private readonly Handler _sut;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ILogger<Handler> _logger = Substitute.For<ILogger<Handler>>();

    public HandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _sut = new Handler(_productRepository, unitOfWork, _logger);
    }

    [Fact]
    public async Task Handle_WithValidProduct_ShouldCreateProductAndReturnSuccessResponse()
    {
        // Arrange
        var request = ProductFaker.CreateValidCommand();

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(request.Price, result.Data.Price);
        Assert.Equal(request.Description, result.Data.Description);
        Assert.Equal(request.Category, result.Data.Category);
        Assert.Equal(request.Image, result.Data.Image);

        await _productRepository.Received(1).Create(Arg.Is<Product>(p => p.Price == request.Price &&
                                                                         p.Description == request.Description &&
                                                                         p.Category == request.Category &&
                                                                         p.Image == request.Image),
                                                    Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = ProductFaker.CreateValidCommand();
        var exception = new Exception("Database connection failed");

        _productRepository.Create(Arg.Any<Product>(), Arg.Any<CancellationToken>())
                          .ThrowsAsync(exception);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("An unexpectd error occour while creating sale. Please try again later!", result.Message);

        await _productRepository.Received(1).Create(Arg.Any<Product>(), Arg.Any<CancellationToken>());

        _logger.Received().Log(LogLevel.Error,
                               Arg.Any<EventId>(),
                               Arg.Any<object>(),
                               Arg.Any<Exception>(),
                               Arg.Any<Func<object, Exception, string>>());
    }

    [Fact]
    public async Task Handle_WithInvalidProductData_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = ProductFaker.CreateInValidCommand();

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Message);
        Assert.Contains("Price", result.Message);

        await _productRepository.DidNotReceiveWithAnyArgs().Create(default!, default);
    }
}