using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Queries;

public record GetActiveDeliveriesQuery : IRequest<IEnumerable<Delivery>>;

public class GetActiveDeliveriesHandler(IDeliveryRepository deliveryRepository)
    : IRequestHandler<GetActiveDeliveriesQuery, IEnumerable<Delivery>>
{
    public async Task<IEnumerable<Delivery>> Handle(GetActiveDeliveriesQuery request, CancellationToken cancellationToken)
    {
        return await deliveryRepository.GetActiveDeliveries();
    }
}
