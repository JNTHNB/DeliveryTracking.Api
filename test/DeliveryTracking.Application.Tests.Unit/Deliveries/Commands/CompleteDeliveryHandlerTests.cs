using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Application.Interfaces;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Commands;

public class CompleteDeliveryHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly CompleteDeliveryHandler _handler;

    public CompleteDeliveryHandlerTests()
    {
        _handler = new CompleteDeliveryHandler(_deliveryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldSetStatusToCompleted()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var delivery = new Delivery(deliveryId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        delivery.Start();
        delivery.ClearDomainEvents();
        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);

        var command = new CompleteDeliveryCommand(deliveryId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        delivery.Status.Should().Be(DeliveryStatus.Completed);
        delivery.CompletedAt.Should().NotBeNull();
        delivery.Events.Should().Contain(e => e.Type == DeliveryEventType.DeliveryCompleted);
        _deliveryRepoMock.Verify(r => r.Update(delivery), Times.Once);
        _dispatcherMock.Verify(d => d.Dispatch(It.IsAny<DeliveryCompletedDomainEvent>()), Times.Once);
    }
}


