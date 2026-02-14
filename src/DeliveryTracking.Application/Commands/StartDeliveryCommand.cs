using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.Interfaces;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record StartDeliveryCommand(Guid Id) : IRequest;

public class StartDeliveryHandler(
    IDeliveryRepository deliveryRepository)
    : IRequestHandler<StartDeliveryCommand>
{
    public async Task Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
    {
        var delivery = await deliveryRepository.Find(request.Id);
        if (delivery == null)
        {
            throw new IdNotFoundException("Delivery", request.Id);
        }

        delivery.Start();

        await deliveryRepository.Update(delivery);
    }
}