using System.Linq;
using DeliveryTracking.Application.Models;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Application.Queries;
using DeliveryTracking.Application.Exceptions;
using FluentAssertions;
using Moq;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Queries;

public class GetDeliverySummaryHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly Mock<IDriverRepository> _driverRepoMock = new();
    private readonly Mock<IVehicleRepository> _vehicleRepoMock = new();
    private readonly Mock<IRouteRepository> _routeRepoMock = new();
    private readonly GetDeliverySummaryHandler _handler;

    public GetDeliverySummaryHandlerTests()
    {
        _handler = new GetDeliverySummaryHandler(
            _deliveryRepoMock.Object,
            _driverRepoMock.Object,
            _vehicleRepoMock.Object,
            _routeRepoMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidDeliveryId_ShouldReturnCompleteDeliverySummary()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var routeId = Guid.NewGuid();

        var delivery = new Delivery(deliveryId, driverId, vehicleId, routeId);
        delivery.Start();
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Checkpoint 1 reached", "Bridge A");
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Checkpoint 2 reached", "Tunnel B");
        delivery.Complete();

        var driver = new Driver { Id = driverId, Name = "Alice Smith" };
        var vehicle = new Vehicle { Id = vehicleId, Name = "Falcon-9", Type = VehicleType.RocketVan };
        var route = new Route { Id = routeId, Name = "Interstellar Express", Origin = "Earth", Destination = "Mars" };

        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);
        _driverRepoMock.Setup(r => r.Find(driverId)).ReturnsAsync(driver);
        _vehicleRepoMock.Setup(r => r.Find(vehicleId)).ReturnsAsync(vehicle);
        _routeRepoMock.Setup(r => r.Find(routeId)).ReturnsAsync(route);

        var query = new GetDeliverySummaryQuery(deliveryId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new DeliverySummary
        {
            DeliveryId = deliveryId,
            Driver = new DeliverySummaryDriver(driver.Name),
            Vehicle = new DeliverySummaryVehicle(vehicle.Name, vehicle.Type),
            Route = new DeliverySummaryRoute(route.Name, route.Origin, route.Destination),
            Status = DeliveryStatus.Completed,
            StartedAt = delivery.StartedAt,
            CompletedAt = delivery.CompletedAt,
            Events = delivery.Events.Select(e => new DeliverySummaryEvent(
                e.Timestamp,
                e.Type,
                e.Description,
                e.Location)).ToList()
        });

        _deliveryRepoMock.Verify(r => r.Find(deliveryId), Times.Once);
        _driverRepoMock.Verify(r => r.Find(driverId), Times.Once);
        _vehicleRepoMock.Verify(r => r.Find(vehicleId), Times.Once);
        _routeRepoMock.Verify(r => r.Find(routeId), Times.Once);
    }
}