using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;

namespace DeliveryTracking.Infrastructure.Repositories;

public class DeliveryRepository(IDomainEventContext domainEventContext)
    : InMemoryRepository<Delivery>(domainEventContext), IDeliveryRepository
{
    public Task<IEnumerable<Delivery>> GetActiveDeliveries()
    {
        return Task.FromResult(Entities.Values.Where(d => d.Status is DeliveryStatus.Pending or DeliveryStatus.InProgress));
    }
}