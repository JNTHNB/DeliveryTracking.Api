using DeliveryTracking.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Infrastructure.Events;

public class LoggingDomainEventDispatcher(ILogger<LoggingDomainEventDispatcher> logger) : IDomainEventDispatcher
{
    public Task Dispatch(IDomainEvent domainEvent)
    {
        logger.LogInformation("Domain Event Dispatched: {EventName} at {OccurredOn}", domainEvent.GetType().Name, domainEvent.OccurredOn);
        return Task.CompletedTask;
    }
}

