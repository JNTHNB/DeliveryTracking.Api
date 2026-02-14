using DeliveryTracking.Domain.Aggregates;

namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryStartedDomainEvent(Delivery Delivery) : DomainEvent;