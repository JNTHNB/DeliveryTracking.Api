using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.ValueObjects;
using FluentAssertions;

namespace DeliveryTracking.Domain.Tests.Unit.Aggregates;

public class DeliveryTests
{
    private readonly Guid _deliveryId = Guid.NewGuid();
    private readonly Guid _driverId = Guid.NewGuid();
    private readonly Guid _vehicleId = Guid.NewGuid();
    private readonly Guid _routeId = Guid.NewGuid();

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);

        // Assert
        delivery.Id.Should().Be(_deliveryId);
        delivery.DriverId.Should().Be(_driverId);
        delivery.VehicleId.Should().Be(_vehicleId);
        delivery.RouteId.Should().Be(_routeId);
        delivery.Status.Should().Be(DeliveryStatus.Pending);
        delivery.Events.Should().BeEmpty();
        delivery.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Start_ShouldChangeStatusToInProgressAndRecordEvents_WhenPending()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);

        // Act
        delivery.Start();

        // Assert
        delivery.Status.Should().Be(DeliveryStatus.InProgress);
        delivery.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        delivery.Events.Should().ContainSingle(e => e.Type == DeliveryEventType.RouteStarted);
        delivery.DomainEvents.Should().ContainSingle(e => e is DeliveryStartedDomainEvent);
    }

    [Fact]
    public void Start_ShouldThrowException_WhenNotPending()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);
        delivery.Start();

        // Act
        var act = () => delivery.Start();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Only pending deliveries can be started.");
    }

    [Fact]
    public void LogEvent_ShouldAddEventAndDomainEvent_WhenInProgress()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);
        delivery.Start();
        delivery.ClearDomainEvents();

        // Act
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "At point A", "Point A");

        // Assert
        delivery.Events.Should().Contain(e => 
            e.Type == DeliveryEventType.CheckpointReached && 
            e.Description == "At point A" && 
            e.Location == "Point A");
        delivery.DomainEvents.Should().ContainSingle(e => e is DeliveryEventLoggedDomainEvent);
    }

    [Fact]
    public void LogEvent_ShouldThrowException_WhenCompleted()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);
        delivery.Start();
        delivery.Complete();

        // Act
        var act = () => delivery.LogEvent(DeliveryEventType.Incident, "Flat tire", null);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot log events for a finished delivery.");
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompletedAndRecordEvents_WhenInProgress()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);
        delivery.Start();
        delivery.ClearDomainEvents();

        // Act
        delivery.Complete();

        // Assert
        delivery.Status.Should().Be(DeliveryStatus.Completed);
        delivery.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        delivery.Events.Should().Contain(e => e.Type == DeliveryEventType.DeliveryCompleted);
        delivery.DomainEvents.Should().ContainSingle(e => e is DeliveryCompletedDomainEvent);
    }

    [Fact]
    public void Complete_ShouldThrowException_WhenNotInProgress()
    {
        // Arrange
        var delivery = new Delivery(_deliveryId, _driverId, _vehicleId, _routeId);

        // Act
        var act = () => delivery.Complete();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Only deliveries in progress can be completed.");
    }
}