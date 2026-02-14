using DeliveryTracking.Domain.DomainEvents;

namespace DeliveryTracking.Domain.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(IDomainEvent domainEvent);
}

