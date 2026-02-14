using DeliveryTracking.Domain.Aggregates;

namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryCompletedDomainEvent(Delivery Delivery) : DomainEvent;