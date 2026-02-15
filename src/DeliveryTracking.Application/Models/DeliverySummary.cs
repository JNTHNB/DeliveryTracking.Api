using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Application.Models;

public record DeliverySummary
{
    public Guid DeliveryId { get; init; }
    public required DeliverySummaryDriver Driver { get; init; }
    public required DeliverySummaryVehicle Vehicle { get; init; }
    public required DeliverySummaryRoute Route { get; init; }
    public DeliveryStatus Status { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public List<DeliverySummaryEvent> Events { get; init; } = [];
}

public record DeliverySummaryDriver(string Name);

public record DeliverySummaryVehicle(string Name, VehicleType Type);

public record DeliverySummaryRoute(string Name, string Origin, string Destination);

public record DeliverySummaryEvent(
    DateTimeOffset Timestamp,
    DeliveryEventType Type,
    string Description,
    string? Location);