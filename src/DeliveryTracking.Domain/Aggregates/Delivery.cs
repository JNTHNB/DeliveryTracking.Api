using DeliveryTracking.Domain.DomainEvents;
using DeliveryTracking.Domain.Exceptions;
using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.Aggregates;

public class Delivery(Guid id, Guid driverId, Guid vehicleId, Guid routeId)
    : AggregateRoot
{
    public Guid Id { get; private set; } = id;
    public Guid DriverId { get; private set; } = driverId;
    public Guid VehicleId { get; private set; } = vehicleId;
    public Guid RouteId { get; private set; } = routeId;
    public DeliveryStatus Status { get; private set; } = DeliveryStatus.Pending;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    private readonly List<DeliveryEvent> _events = [];

    public IReadOnlyCollection<DeliveryEvent> Events => _events.AsReadOnly();

    public void Start()
    {
        if (Status != DeliveryStatus.Pending)
        {
            throw new DeliveryAlreadyStartedException();
        }

        Status = DeliveryStatus.InProgress;
        StartedAt = DateTimeOffset.UtcNow;

        RecordEvent(DeliveryEventType.RouteStarted, "Delivery started", null, StartedAt);

        AddDomainEvent(new DeliveryStartedDomainEvent(Id, StartedAt));
    }

    public void LogEvent(DeliveryEventType type, string description, string? location)
    {
        if (Status is DeliveryStatus.Completed or DeliveryStatus.Cancelled)
        {
            throw new DeliveryAlreadyFinishedException();
        }

        var @event = RecordEvent(type, description, location);
        AddDomainEvent(new DeliveryEventLoggedDomainEvent(Id, @event));
    }

    public void Complete()
    {
        if (Status != DeliveryStatus.InProgress)
        {
            throw new DeliveryNotInProgressException();
        }

        Status = DeliveryStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;

        RecordEvent(DeliveryEventType.DeliveryCompleted, "Delivery completed", null, CompletedAt.Value);

        AddDomainEvent(new DeliveryCompletedDomainEvent(Id, CompletedAt.Value));
    }

    private DeliveryEvent RecordEvent(DeliveryEventType type, string description, string? location = null, DateTimeOffset? timestamp = null)
    {
        var @event = new DeliveryEvent
        {
            DeliveryId = Id,
            Timestamp = timestamp ?? DateTimeOffset.UtcNow,
            Type = type,
            Description = description,
            Location = location
        };

        _events.Add(@event);
        return @event;
    }
}


