using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Domain.Aggregates;

// Note: For this POC, thread-safety is not enforced because aggregates
// are typically scoped per request and not shared concurrently.
// In a production system with multiple delivery updates in parallel,
// consider optimistic concurrency or unit-of-work patterns via EF Core.
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}



