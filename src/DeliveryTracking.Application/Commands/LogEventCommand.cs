using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.ValueObjects;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record LogEventCommand(DeliveryEventType Type, string Description, string? Location) : IRequest
{
    public Guid DeliveryId { get; set; }
}

public class LogEventHandler(
    IDeliveryRepository deliveryRepository)
    : IRequestHandler<LogEventCommand>
{
    public async Task Handle(LogEventCommand request, CancellationToken cancellationToken)
    {
        var delivery = await deliveryRepository.Find(request.DeliveryId);
        if (delivery == null) throw new IdNotFoundException("Delivery", request.DeliveryId);

        delivery.LogEvent(request.Type, request.Description, request.Location);

        await deliveryRepository.Update(delivery);
    }
}


