using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Application.Queries;

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
    public async Task Handle_ShouldReturnSummary()
    {
        // Arrange
        var deliveryId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var vehicleId = Guid.NewGuid();
        var routeId = Guid.NewGuid();
        
        var delivery = new Delivery(deliveryId, driverId, vehicleId, routeId);
        var driver = new Driver { Id = driverId, Name = "John Doe" };
        var vehicle = new Vehicle { Id = vehicleId, Name = "Truck 1" };
        var route = new Route { Id = routeId, Name = "Route 66" };

        _deliveryRepoMock.Setup(r => r.Find(deliveryId)).ReturnsAsync(delivery);
        _driverRepoMock.Setup(r => r.Find(driverId)).ReturnsAsync(driver);
        _vehicleRepoMock.Setup(r => r.Find(vehicleId)).ReturnsAsync(vehicle);
        _routeRepoMock.Setup(r => r.Find(routeId)).ReturnsAsync(route);

        var query = new GetDeliverySummaryQuery(deliveryId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.DeliveryId.Should().Be(deliveryId);
        result.Driver.Name.Should().Be("John Doe");
        result.Vehicle.Name.Should().Be("Truck 1");
        result.Route.Name.Should().Be("Route 66");
    }
}


