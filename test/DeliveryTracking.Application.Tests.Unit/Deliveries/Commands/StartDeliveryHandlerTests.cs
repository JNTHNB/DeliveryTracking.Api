using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Application.Interfaces;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Commands;

public class StartDeliveryHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly StartDeliveryHandler _handler;

    public StartDeliveryHandlerTests()
    {
        _handler = new StartDeliveryHandler(_deliveryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldStartPendingDelivery()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var delivery = new Delivery(deliveryId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);

        var command = new StartDeliveryCommand(deliveryId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        delivery.Status.Should().Be(DeliveryStatus.InProgress);
        delivery.Events.Should().ContainSingle(e => e.Type == DeliveryEventType.RouteStarted);
        _deliveryRepoMock.Verify(r => r.Update(delivery), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDeliveryNotFound()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync((Delivery?)null);

        var command = new StartDeliveryCommand(deliveryId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage($"Delivery Id {deliveryId} invalid");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDeliveryAlreadyStarted()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var delivery = new Delivery(deliveryId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        delivery.Start();
        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);

        var command = new StartDeliveryCommand(deliveryId);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Only pending deliveries can be started.");
    }
}


