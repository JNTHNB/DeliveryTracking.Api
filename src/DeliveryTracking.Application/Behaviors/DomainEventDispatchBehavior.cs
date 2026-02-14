using DeliveryTracking.Domain.Interfaces;
using MediatR;

namespace DeliveryTracking.Application.Behaviors;

public class DomainEventDispatchBehavior<TRequest, TResponse>(
    IDomainEventContext context,
    IDomainEventDispatcher dispatcher)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        var aggregates = context.GetRegisteredAggregates();

        foreach (var aggregate in aggregates)
        {
            var domainEvents = aggregate.DomainEvents.ToList();
            aggregate.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await dispatcher.Dispatch(domainEvent);
            }
        }

        context.Clear();

        return response;
    }
}