using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record CreateDeliveryCommand(Guid DriverId, Guid VehicleId, Guid RouteId) : IRequest<Delivery>;

public class CreateDeliveryHandler(
    IDeliveryRepository deliveryRepository,
    IDriverRepository driverRepository,
    IVehicleRepository vehicleRepository,
    IRouteRepository routeRepository)
    : IRequestHandler<CreateDeliveryCommand, Delivery>
{
    public async Task<Delivery> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (await driverRepository.Find(request.DriverId) == null)
            throw new IdNotFoundException("Driver", request.DriverId);

        if (await vehicleRepository.Find(request.VehicleId) == null)
            throw new IdNotFoundException("Vehicle", request.VehicleId);

        if (await routeRepository.Find(request.RouteId) == null)
            throw new IdNotFoundException("Route", request.RouteId);

        var delivery = new Delivery(Guid.NewGuid(), request.DriverId, request.VehicleId, request.RouteId);

        await deliveryRepository.Add(delivery);

        return delivery;
    }
}


