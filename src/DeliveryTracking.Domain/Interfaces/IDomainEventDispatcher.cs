namespace DeliveryTracking.Domain.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(IDomainEvent domainEvent);
}

