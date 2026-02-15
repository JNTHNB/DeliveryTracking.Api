namespace DeliveryTracking.Domain.Interfaces;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}