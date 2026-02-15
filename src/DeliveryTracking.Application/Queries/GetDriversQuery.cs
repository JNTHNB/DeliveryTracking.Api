using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Queries;

public record GetDriversQuery : IRequest<IEnumerable<Driver>>;

public class GetDriversHandler(IDriverRepository driverRepository)
    : IRequestHandler<GetDriversQuery, IEnumerable<Driver>>
{
    public async Task<IEnumerable<Driver>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
    {
        return await driverRepository.List();
    }
}
