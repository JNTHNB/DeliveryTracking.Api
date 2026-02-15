using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Queries;

public record GetRoutesQuery : IRequest<IEnumerable<Route>>;

public class GetRoutesHandler(IRouteRepository routeRepository)
    : IRequestHandler<GetRoutesQuery, IEnumerable<Route>>
{
    public async Task<IEnumerable<Route>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        return await routeRepository.List();
    }
}
