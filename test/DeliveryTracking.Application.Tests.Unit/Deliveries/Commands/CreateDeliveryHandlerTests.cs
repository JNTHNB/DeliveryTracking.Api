using DeliveryTracking.Application.Commands;
using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Application.Interfaces;

namespace DeliveryTracking.Application.Tests.Unit.Deliveries.Commands;

public class CreateDeliveryHandlerTests
{
    private readonly Mock<IDeliveryRepository> _deliveryRepoMock = new();
    private readonly Mock<IDriverRepository> _driverRepoMock = new();
    private readonly Mock<IVehicleRepository> _vehicleRepoMock = new();
    private readonly Mock<IRouteRepository> _routeRepoMock = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly CreateDeliveryHandler _handler;

    public CreateDeliveryHandlerTests()
    {
        _handler = new CreateDeliveryHandler(
            _deliveryRepoMock.Object,
            _driverRepoMock.Object,
            _vehicleRepoMock.Object,
            _routeRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateDeliveryInPendingStatus()
    {
        // Arrange
        var command = new CreateDeliveryCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _driverRepoMock.Setup(r => r.Find(command.DriverId)).ReturnsAsync(new Driver { Id = command.DriverId });
        _vehicleRepoMock.Setup(r => r.Find(command.VehicleId)).ReturnsAsync(new Vehicle { Id = command.VehicleId });
        _routeRepoMock.Setup(r => r.Find(command.RouteId)).ReturnsAsync(new Route { Id = command.RouteId });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.DriverId.Should().Be(command.DriverId);
        result.Status.Should().Be(DeliveryStatus.Pending);
        result.Events.Should().BeEmpty();
        _deliveryRepoMock.Verify(r => r.Add(It.IsAny<Delivery>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowIdNotFoundException_WhenDriverNotFound()
    {
        // Arrange
        var command = new CreateDeliveryCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _driverRepoMock.Setup(r => r.Find(command.DriverId)).ReturnsAsync((Driver?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<IdNotFoundException>()
            .WithMessage($"Driver Id {command.DriverId} invalid");
    }

    [Fact]
    public async Task Handle_ShouldThrowIdNotFoundException_WhenVehicleNotFound()
    {
        // Arrange
        var command = new CreateDeliveryCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _driverRepoMock.Setup(r => r.Find(command.DriverId)).ReturnsAsync(new Driver { Id = command.DriverId });
        _vehicleRepoMock.Setup(r => r.Find(command.VehicleId)).ReturnsAsync((Vehicle?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<IdNotFoundException>()
            .WithMessage($"Vehicle Id {command.VehicleId} invalid");
    }

    [Fact]
    public async Task Handle_ShouldThrowIdNotFoundException_WhenRouteNotFound()
    {
        // Arrange
        var command = new CreateDeliveryCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _driverRepoMock.Setup(r => r.Find(command.DriverId)).ReturnsAsync(new Driver { Id = command.DriverId });
        _vehicleRepoMock.Setup(r => r.Find(command.VehicleId)).ReturnsAsync(new Vehicle { Id = command.VehicleId });
        _routeRepoMock.Setup(r => r.Find(command.RouteId)).ReturnsAsync((Route?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<IdNotFoundException>()
            .WithMessage($"Route Id {command.RouteId} invalid");
    }
}


