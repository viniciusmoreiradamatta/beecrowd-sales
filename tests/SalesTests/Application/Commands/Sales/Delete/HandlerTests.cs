using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SalesApplication.Commands.Sales.Delete;
using SalesDomain.Entities.Sale;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesTests.Application.Commands.Sales.Delete
{
    public class HandlerTests
    {
        private readonly Handler _sut;
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly ILogger<Handler> _logger = Substitute.For<ILogger<Handler>>();

        public HandlerTests() => _sut = new(_saleRepository, unitOfWork, _logger);

        [Fact]
        public async Task Handle_WithExistingSale_ShouldCancelAndNotify()
        {
            // Arrange
            var command = new Command(Guid.NewGuid());

            var sale = SaleFaker.CreateSale();

            var expectedDate = DateTime.UtcNow;

            _saleRepository.GetById(command.Id, Arg.Any<CancellationToken>()).Returns(sale);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Equal("Sell Cancelled", result.Message);

            await _saleRepository.Received(1).GetById(command.Id, Arg.Any<CancellationToken>());

            Assert.True(sale.Cancelled);

            _logger.DidNotReceiveWithAnyArgs().Log(LogLevel.Error,
                                                   Arg.Any<EventId>(),
                                                   Arg.Any<object>(),
                                                   Arg.Any<Exception>(),
                                                   Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_ShouldLogButReturnSuccess()
        {
            // Arrange
            var command = new Command(Guid.NewGuid());

            var exception = new Exception("Randon error");

            _saleRepository.GetById(command.Id, Arg.Any<CancellationToken>()).ThrowsAsync(exception);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal("An unexpectd error occour while canceling sell. Please try again later!", result.Message);

            _logger.Received(1).Log(LogLevel.Error,
                                    Arg.Any<EventId>(),
                                    Arg.Any<object>(),
                                    Arg.Is<Exception>(e => e.Message == "Randon error"),
                                    Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task Handle_WithNonExistentSale_ShouldReturnError()
        {
            // Arrange
            var command = new Command(Guid.NewGuid());

            Sale? sale = null;

            _saleRepository.GetById(command.Id, Arg.Any<CancellationToken>()).Returns(sale);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);

            Assert.Equal("Sell not found", result.Message);

            await _saleRepository.Received(1).GetById(command.Id, Arg.Any<CancellationToken>());
        }
    }
}