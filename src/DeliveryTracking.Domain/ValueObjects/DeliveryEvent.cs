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
    public DateTime Timestamp { get; set; }
    public DeliveryEventType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
}


