namespace DeliveryTracking.Domain.Aggregates;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public VehicleType Type { get; init; }
}