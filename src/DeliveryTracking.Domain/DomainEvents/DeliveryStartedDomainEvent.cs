namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryStartedDomainEvent(Guid DeliveryId, DateTimeOffset StartedAt) : DomainEvent;