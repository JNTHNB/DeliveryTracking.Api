using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record CreateVehicleCommand(string Name, VehicleType Type) : IRequest<Vehicle>;

public class CreateVehicleHandler(IVehicleRepository vehicleRepository)
    : IRequestHandler<CreateVehicleCommand, Vehicle>
{
    public async Task<Vehicle> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type
        };

        await vehicleRepository.Add(vehicle);

        return vehicle;
    }
}
