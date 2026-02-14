using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using Route = DeliveryTracking.Domain.Aggregates.Route;

namespace DeliveryTracking.Api;

public static class DataSeeder
{
    public static void SeedData(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var driverRepo = scope.ServiceProvider.GetRequiredService<IDriverRepository>();
        var vehicleRepo = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();
        var routeRepo = scope.ServiceProvider.GetRequiredService<IRouteRepository>();

        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        driverRepo.Add(new Driver { Id = driverId, Name = "Big Bob" }).Wait();

        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        vehicleRepo.Add(new Vehicle { Id = vehicleId, Name = "Van1", Type = VehicleType.RocketVan }).Wait();

        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        routeRepo.Add(new Route
        {
            Id = routeId,
            Name = "ExpressTruck",
            Origin = "Auckland",
            Destination = "Wellington",
            Checkpoints =
            [
                new Checkpoint { Id = Guid.NewGuid(), Name = "Asteroid Belt", Sequence = 1 },
                new Checkpoint { Id = Guid.NewGuid(), Name = "Nebula Pass", Sequence = 2 }
            ]
        }).Wait();
    }
}



