namespace DeliveryTracking.Domain.Aggregates;

public class Driver
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
}