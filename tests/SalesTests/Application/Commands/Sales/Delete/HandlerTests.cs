using Microsoft.Extensions.Logging;
using NSubstitute;
using SalesApplication.Commands.Sales.Delete;
using SalesDomain.Abstractions;
using SalesDomain.Entities.Sale;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesTests.Application.Commands.Sales.Delete
{
    public class HandlerTests
    {
        private readonly Handler _sut;
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IProducer _producer = Substitute.For<IProducer>();
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly ILogger<Handler> _logger = Substitute.For<ILogger<Handler>>();

        public HandlerTests() => _sut = new(_saleRepository, unitOfWork, _producer, _dateTimeProvider, _logger);

        [Fact]
        public async Task Handle_WithExistingSale_ShouldCancelAndNotify()
        {
            // Arrange
            var command = new Command(Guid.NewGuid());

            var sale = SaleFaker.CreateSale();

            var expectedDate = DateTime.UtcNow;

            _saleRepository.GetById(command.Id, Arg.Any<CancellationToken>()).Returns(sale);

            _dateTimeProvider.UtcNow.Returns(expectedDate);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            Assert.Equal("Sell Cancelled", result.Message);

            await _saleRepository.Received(1).GetById(command.Id, Arg.Any<CancellationToken>());

            Assert.True(sale.Cancelled);

            await _producer.Received(1).Notify(Arg.Is<SaleCancelledEvent>(e => e.OccurredOn == expectedDate && e.Id == command.Id));

            _logger.DidNotReceiveWithAnyArgs().Log(LogLevel.Error,
                                                   Arg.Any<EventId>(),
                                                   Arg.Any<object>(),
                                                   Arg.Any<Exception>(),
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

            await _producer.DidNotReceiveWithAnyArgs().Notify(Arg.Any<SaleCancelledEvent>());
        }
    }
}