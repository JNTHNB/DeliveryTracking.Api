using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Domain.DomainEvents;

public record DeliveryEventLoggedDomainEvent(Guid DeliveryId, DeliveryEvent NewEvent) : DomainEvent;