using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Application.Interfaces;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Commands;

public class LogEventHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly LogEventHandler _handler;

    public LogEventHandlerTests()
    {
        _handler = new LogEventHandler(_deliveryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddEventToDelivery()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var delivery = new Delivery(deliveryId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        delivery.Start();
        delivery.ClearDomainEvents();
        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);

        var command = new LogEventCommand(DeliveryEventType.Incident, "Asteroid hit!", "Deep Space") { DeliveryId = deliveryId };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        delivery.Events.Should().HaveCount(2); // 1 from Start() + 1 from LogEvent()
        delivery.Events.Should().Contain(e => e.Type == DeliveryEventType.Incident && e.Description == "Asteroid hit!");
        _deliveryRepoMock.Verify(r => r.Update(delivery), Times.Once);
    }
}


