using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Domain.DomainEvents;

public abstract record DomainEvent : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}