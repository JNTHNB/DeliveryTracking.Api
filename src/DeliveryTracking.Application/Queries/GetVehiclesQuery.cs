using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Queries;

public record GetVehiclesQuery : IRequest<IEnumerable<Vehicle>>;

public class GetVehiclesHandler(IVehicleRepository vehicleRepository)
    : IRequestHandler<GetVehiclesQuery, IEnumerable<Vehicle>>
{
    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        return await vehicleRepository.List();
    }
}
