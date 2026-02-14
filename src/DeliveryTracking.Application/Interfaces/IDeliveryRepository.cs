using DeliveryTracking.Domain.Aggregates;

namespace DeliveryTracking.Application.Interfaces;

public interface IDeliveryRepository : IRepository<Delivery>
{
    Task<IEnumerable<Delivery>> GetActiveDeliveries();
}

