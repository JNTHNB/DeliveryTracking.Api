using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record CreateDriverCommand(string Name) : IRequest<Driver>;

public class CreateDriverHandler(IDriverRepository driverRepository)
    : IRequestHandler<CreateDriverCommand, Driver>
{
    public async Task<Driver> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await driverRepository.Add(driver);

        return driver;
    }
}
