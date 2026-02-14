using DeliveryTracking.Domain.Aggregates;

namespace DeliveryTracking.Domain.Interfaces;

public interface IDomainEventContext
{
    void RegisterAggregate(AggregateRoot aggregate);
    IReadOnlyCollection<AggregateRoot> GetRegisteredAggregates();
    void Clear();
}
