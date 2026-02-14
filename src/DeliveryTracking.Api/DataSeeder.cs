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
        var deliveryRepo = scope.ServiceProvider.GetRequiredService<IDeliveryRepository>();

        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        driverRepo.Add(new Driver { Id = driverId, Name = "Big Bob" }).Wait();

        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        vehicleRepo.Add(new Vehicle { Id = vehicleId, Name = "Van1", Type = VehicleType.RocketVan }).Wait();

        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        routeRepo.Add(new Route
        {
            Id = routeId,
            Name = "SH1 Southbound",
            Origin = "Auckland",
            Destination = "Wellington",
            Checkpoints =
            [
                new Checkpoint { Id = Guid.NewGuid(), Name = "Hamilton", Sequence = 1 },
                new Checkpoint { Id = Guid.NewGuid(), Name = "Taupo", Sequence = 2 },
                new Checkpoint { Id = Guid.NewGuid(), Name = "Palmerston North", Sequence = 3 },
                new Checkpoint { Id = Guid.NewGuid(), Name = "Kapiti Coast", Sequence = 4 }
            ]
        }).Wait();

        var deliveryId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        var delivery = new Delivery(deliveryId, driverId, vehicleId, routeId);
        delivery.Start();
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Arrived at Hamilton", "Hamilton");
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Coffee break at Taupo", "Taupo");
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Arrived at Palmerston North", "Palmerston North");
        delivery.LogEvent(DeliveryEventType.CheckpointReached, "Driving through Kapiti Coast", "Kapiti Coast");
        delivery.Complete();
        deliveryRepo.Add(delivery).Wait();
    }
}



