using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record CompleteDeliveryCommand(Guid DeliveryId) : IRequest;

public class CompleteDeliveryHandler(
    IDeliveryRepository deliveryRepository)
    : IRequestHandler<CompleteDeliveryCommand>
{
    public async Task Handle(CompleteDeliveryCommand request, CancellationToken cancellationToken)
    {
        var delivery = await deliveryRepository.Find(request.DeliveryId);
        if (delivery == null) throw new IdNotFoundException("Delivery", request.DeliveryId);

        delivery.Complete();

        await deliveryRepository.Update(delivery);
    }
}