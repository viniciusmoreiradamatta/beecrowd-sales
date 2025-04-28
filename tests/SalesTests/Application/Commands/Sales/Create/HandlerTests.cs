using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SalesApplication.Commands.Sales.Create;
using SalesDomain.Abstractions;
using SalesDomain.Entities.Sale;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;

namespace SalesTests.Application.Commands.Sales.Create
{
    public class HandlerTests
    {
        private readonly Handler _sut;

        private readonly ILogger<Handler> _logger = Substitute.For<ILogger<Handler>>();
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        public HandlerTests()
        {
            _sut = new Handler(_saleRepository, unitOfWork, dateTimeProvider, _logger);
        }

        [Fact]
        public async Task Handle_WithInvalidSale_ShouldReturnValidationErrors()
        {
            // Arrange
            var request = SaleFaker.CreateValidRequest();
            request.Items[0].Quantity = 0;

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("errors occour during validation sale", result.Message);

            await _saleRepository.DidNotReceiveWithAnyArgs().Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_ShouldLogErrorAndReturnFailure()
        {
            // Arrange
            var request = SaleFaker.CreateValidRequest();
            var exception = new Exception("Database failure");

            _saleRepository.Create(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                           .ThrowsAsync(exception);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("An unexpectd error occour while creating sale. Please try again later!", result.Message);

            _logger.Received(1).LogError(exception, exception.ToString());
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
            Assert.Equal("Sale succesfuly created", result.Message);
        }
    }
}