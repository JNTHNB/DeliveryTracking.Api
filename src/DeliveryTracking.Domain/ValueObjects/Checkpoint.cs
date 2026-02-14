namespace DeliveryTracking.Domain.ValueObjects;

public class Checkpoint
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sequence { get; set; }
}