namespace DeliveryTracking.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}