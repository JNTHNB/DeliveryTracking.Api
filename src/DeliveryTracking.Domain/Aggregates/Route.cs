using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.Aggregates;

public class Route
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public List<Checkpoint> Checkpoints { get; set; } = new();
}

