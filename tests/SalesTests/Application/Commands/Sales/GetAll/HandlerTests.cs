using NSubstitute;
using SalesApplication.Commands.Sales.GetAll;
using SalesDomain.Interfaces.Repository;

namespace SalesTests.Application.Commands.Sales.GetAll;

public class HandlerTests
{
    private readonly Handler _sut;
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();

    public HandlerTests()
    {
        _sut = new(_saleRepository);
    }

    [Fact]
    public async Task OK()
    {
        // Arrange
        var list = Enumerable.Range(0, 5).Select(c => SaleFaker.CreateSale()).ToList();

        _saleRepository.GetAll(Arg.Any<CancellationToken>()).Returns(list);

        // Act
        var sut = await _sut.Handle(new Command(), CancellationToken.None);

        // Assert
        Assert.True(sut.IsSuccess);

        Assert.NotEmpty(sut.Data);
    }
}