using Microsoft.Extensions.Logging;
using NSubstitute;
using SalesApplication.Commands.Sales.Create;
using SalesDomain.Abstractions;
using SalesDomain.Entities.Sale;
using SalesDomain.Events.Sales;
using SalesDomain.Interfaces.Message;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesTests.Application.Commands.Sales.Create
{
    public class HandlerTests
    {
        private readonly Handler _sut;

        private readonly IProducer _producer = Substitute.For<IProducer>();
        private readonly ILogger<Handler> _logger = Substitute.For<ILogger<Handler>>();
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        public HandlerTests()
        {
            _sut = new Handler(_saleRepository, unitOfWork, _dateTimeProvider, _producer, _logger);
        }

        [Fact]
        public async Task Handle_WithInvalidSale_ShouldReturnValidationErrors()
        {
            // Arrange
            var request = SaleFaker.CreateValidRequest();
            request.Items[0].ProductId = Guid.Empty;

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid Sell", result.Message);

            await _saleRepository.DidNotReceiveWithAnyArgs().Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
            await _producer.DidNotReceiveWithAnyArgs().Notify(Arg.Any<SaleCreatedEvent>());
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldCreateSaleAndReturnSuccess()
        {
            // Arrange
            var request = SaleFaker.CreateValidRequest();

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Sale successfully created", result.Message);
        }
    }
}