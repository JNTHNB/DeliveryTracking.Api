using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using MediatR;

namespace DeliveryTracking.Application.Commands;

public record CreateRouteCommand(string Name, string Origin, string Destination, List<Checkpoint> Checkpoints) : IRequest<Route>;

public class CreateRouteHandler(IRouteRepository routeRepository)
    : IRequestHandler<CreateRouteCommand, Route>
{
    public async Task<Route> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new Route
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Origin = request.Origin,
            Destination = request.Destination,
            Checkpoints = request.Checkpoints
        };

        await routeRepository.Add(route);

        return route;
    }
}
