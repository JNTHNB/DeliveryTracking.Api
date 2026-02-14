using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Application.Queries;
using DeliveryTracking.Domain.Aggregates;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Queries;

public class GetActiveDeliveriesHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly GetActiveDeliveriesHandler _handler;

    public GetActiveDeliveriesHandlerTests()
    {
        _handler = new GetActiveDeliveriesHandler(_deliveryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnActiveDeliveriesFromRepository()
    {
        // Arrange
        var deliveries = new List<Delivery>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        };

        _deliveryRepoMock.Setup(r => r.GetActiveDeliveries())
            .ReturnsAsync(deliveries);

        var query = new GetActiveDeliveriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(deliveries);
        _deliveryRepoMock.Verify(r => r.GetActiveDeliveries(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoActiveDeliveries_ShouldReturnEmptyList()
    {
        // Arrange
        var deliveries = new List<Delivery>();

        _deliveryRepoMock.Setup(r => r.GetActiveDeliveries())
            .ReturnsAsync(deliveries);

        var query = new GetActiveDeliveriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _deliveryRepoMock.Verify(r => r.GetActiveDeliveries(), Times.Once);
    }
}
