using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using DeliveryTracking.Domain.DomainEvents;

namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryCompletedDomainEvent(Delivery Delivery) : DomainEvent;




