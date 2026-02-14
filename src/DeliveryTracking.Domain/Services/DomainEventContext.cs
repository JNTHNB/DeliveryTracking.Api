using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using System.Collections.Concurrent;

namespace DeliveryTracking.Domain.Services;

public class DomainEventContext : IDomainEventContext
{
    private readonly ConcurrentDictionary<AggregateRoot, byte> _aggregates = new();

    public void RegisterAggregate(AggregateRoot aggregate)
    {
        _aggregates.TryAdd(aggregate, 0);
    }

    public IReadOnlyCollection<AggregateRoot> GetRegisteredAggregates()
    {
        return _aggregates.Keys.ToList().AsReadOnly();
    }

    public void Clear()
    {
        _aggregates.Clear();
    }
}
