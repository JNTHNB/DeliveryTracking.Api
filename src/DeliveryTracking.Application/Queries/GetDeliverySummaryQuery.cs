using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Application.Models;
using MediatR;

namespace DeliveryTracking.Application.Queries;

public record GetDeliverySummaryQuery(Guid DeliveryId) : IRequest<DeliverySummary?>;

public class GetDeliverySummaryHandler(
    IDeliveryRepository deliveryRepository,
    IDriverRepository driverRepository,
    IVehicleRepository vehicleRepository,
    IRouteRepository routeRepository)
    : IRequestHandler<GetDeliverySummaryQuery, DeliverySummary?>
{
    public async Task<DeliverySummary?> Handle(GetDeliverySummaryQuery request, CancellationToken cancellationToken)
    {
        var delivery = await deliveryRepository.Find(request.DeliveryId);
        if (delivery == null)
        {
            throw new IdNotFoundException("Delivery", request.DeliveryId);
        }

        var driver = await driverRepository.Find(delivery.DriverId);
        var vehicle = await vehicleRepository.Find(delivery.VehicleId);
        var route = await routeRepository.Find(delivery.RouteId);

        return new DeliverySummary
        {
            DeliveryId = delivery.Id,
            Driver = new DeliverySummaryDriver(driver!.Name),
            Vehicle = new DeliverySummaryVehicle(vehicle!.Name, vehicle!.Type),
            Route = new DeliverySummaryRoute(route!.Name, route!.Origin, route!.Destination),
            Status = delivery.Status,
            StartedAt = delivery.StartedAt,
            CompletedAt = delivery.CompletedAt,
            Events = delivery.Events.Select(e => new DeliverySummaryEvent(e.Timestamp, e.Type, e.Description, e.Location)).ToList()
        };
    }
}