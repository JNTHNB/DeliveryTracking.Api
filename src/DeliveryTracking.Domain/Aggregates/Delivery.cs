using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.Aggregates;

public class Delivery(Guid id, Guid driverId, Guid vehicleId, Guid routeId)
    : AggregateRoot
{
    private readonly object _lock = new();

    public Guid Id { get; private set; } = id;
    public Guid DriverId { get; private set; } = driverId;
    public Guid VehicleId { get; private set; } = vehicleId;
    public Guid RouteId { get; private set; } = routeId;
    public DeliveryStatus Status { get; private set; } = DeliveryStatus.Pending;
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    private readonly List<DeliveryEvent> _events = [];

    public IReadOnlyCollection<DeliveryEvent> Events
    {
        get
        {
            lock (_lock)
            {
                return _events.ToList().AsReadOnly();
            }
        }
    }

    public void Start()
    {
        lock (_lock)
        {
            if (Status != DeliveryStatus.Pending)
            {
                throw new InvalidOperationException("Only pending deliveries can be started.");
            }

            Status = DeliveryStatus.InProgress;
            StartedAt = DateTime.UtcNow;

            RecordEvent(DeliveryEventType.RouteStarted, "Delivery started", null, StartedAt);

            AddDomainEvent(new DeliveryStartedDomainEvent(this));
        }
    }

    public void LogEvent(DeliveryEventType type, string description, string? location)
    {
        lock (_lock)
        {
            if (Status is DeliveryStatus.Completed or DeliveryStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot log events for a finished delivery.");
            }

            var @event = RecordEvent(type, description, location);
            AddDomainEvent(new DeliveryEventLoggedDomainEvent(this, @event));
        }
    }

    public void Complete()
    {
        lock (_lock)
        {
            if (Status != DeliveryStatus.InProgress)
            {
                throw new InvalidOperationException("Only deliveries in progress can be completed.");
            }

            Status = DeliveryStatus.Completed;
            CompletedAt = DateTime.UtcNow;

            RecordEvent(DeliveryEventType.DeliveryCompleted, "Delivery completed", null, CompletedAt.Value);

            AddDomainEvent(new DeliveryCompletedDomainEvent(this));
        }
    }

    private DeliveryEvent RecordEvent(DeliveryEventType type, string description, string? location = null, DateTime? timestamp = null)
    {
        var @event = new DeliveryEvent
        {
            Id = Guid.NewGuid(),
            DeliveryId = Id,
            Timestamp = timestamp ?? DateTime.UtcNow,
            Type = type,
            Description = description,
            Location = location
        };

        _events.Add(@event);
        return @event;
    }
}


