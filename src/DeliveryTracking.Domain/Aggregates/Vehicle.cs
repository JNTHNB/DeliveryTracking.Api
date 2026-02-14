namespace DeliveryTracking.Domain.Aggregates;

public enum VehicleType
{
    HoverTruck,
    RocketVan,
    SpaceCycle
}

public class Vehicle
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public VehicleType Type { get; set; }
}


