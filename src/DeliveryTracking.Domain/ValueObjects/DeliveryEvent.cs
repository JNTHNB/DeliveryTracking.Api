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
    public Guid Id { get; set; }
    public Guid DeliveryId { get; set; }
    public DateTime Timestamp { get; init; }
    public DeliveryEventType Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? Location { get; init; }
}