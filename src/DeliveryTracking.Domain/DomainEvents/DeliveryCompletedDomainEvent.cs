namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryCompletedDomainEvent(Guid DeliveryId, DateTimeOffset CompletedAt) : DomainEvent;