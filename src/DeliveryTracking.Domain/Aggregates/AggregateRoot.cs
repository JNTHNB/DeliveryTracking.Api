using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Domain.Aggregates;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly object _lock = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            lock (_lock)
            {
                return _domainEvents.ToList().AsReadOnly();
            }
        }
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        lock (_lock)
        {
            _domainEvents.Add(domainEvent);
        }
    }

    public void ClearDomainEvents()
    {
        lock (_lock)
        {
            _domainEvents.Clear();
        }
    }
}



