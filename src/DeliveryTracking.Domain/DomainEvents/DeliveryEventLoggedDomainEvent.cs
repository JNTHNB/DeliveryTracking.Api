using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryEventLoggedDomainEvent(Delivery Delivery, DeliveryEvent NewEvent) : DomainEvent;