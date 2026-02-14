using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.Aggregates;

public class Route
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public string Origin { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public List<Checkpoint> Checkpoints { get; init; } = [];
}

