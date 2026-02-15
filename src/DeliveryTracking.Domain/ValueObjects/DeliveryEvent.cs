namespace DeliveryTracking.Domain.ValueObjects;

public enum DeliveryEventType
{
    RouteStarted,
    CheckpointReached,
    Incident,
    DeliveryCompleted,
    Other
}

public class DeliveryEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid DeliveryId { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public DeliveryEventType Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
}