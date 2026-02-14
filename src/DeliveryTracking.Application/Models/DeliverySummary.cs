using System.Text;
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
    public DateTime StartedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public List<DeliverySummaryEvent> Events { get; init; } = [];

    public static string Generate(DeliverySummary delivery)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Delivery ID: {delivery.DeliveryId}");
        sb.AppendLine($"Driver: {delivery.Driver.Name}");
        sb.AppendLine($"Vehicle: {delivery.Vehicle.Type}");
        sb.AppendLine($"Route: {delivery.Route.Origin} → {delivery.Route.Destination}");
        sb.AppendLine("Events:");

        foreach (var e in delivery.Events)
        {
            sb.AppendLine(
                $"[{e.Timestamp:u}] {e.Type}: {e.Description}"
            );
        }

        return sb.ToString();
    }
}

public record DeliverySummaryDriver(string Name);

public record DeliverySummaryVehicle(string Name, VehicleType Type);

public record DeliverySummaryRoute(string Name, string Origin, string Destination);

public record DeliverySummaryEvent(
    DateTime Timestamp,
    DeliveryEventType Type,
    string Description,
    string? Location);