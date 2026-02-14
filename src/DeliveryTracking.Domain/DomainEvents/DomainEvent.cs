using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Domain.DomainEvents;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}