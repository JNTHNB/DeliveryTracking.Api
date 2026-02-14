using DeliveryTracking.Application.Models;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Application.Tests.Unit.Models;

public class DeliverySummaryTests
{
    [Fact]
    public void Generate_ShouldReturnFormattedString()
    {
        // Arrange
        var deliveryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var timestamp = new DateTime(2026, 2, 14, 20, 25, 0, DateTimeKind.Utc);
        
        var summary = new DeliverySummary
        {
            DeliveryId = deliveryId,
            Driver = new DeliverySummaryDriver("John Driver"),
            Vehicle = new DeliverySummaryVehicle("Van 1", VehicleType.RocketVan),
            Route = new DeliverySummaryRoute("Main Route", "New York", "Los Angeles"),
            Status = DeliveryStatus.InProgress,
            StartedAt = timestamp,
            CompletedAt = null,
            Events =
            [
                new DeliverySummaryEvent(
                    timestamp,
                    DeliveryEventType.RouteStarted,
                    "Starting the journey",
                    "Warehouse A"
                )
            ]
        };

        // Act
        var result = DeliverySummary.Generate(summary);

        // Assert
        var expected = new System.Text.StringBuilder();
        expected.AppendLine("Delivery ID: 11111111-1111-1111-1111-111111111111");
        expected.AppendLine("Driver: John Driver");
        expected.AppendLine("Vehicle: RocketVan");
        expected.AppendLine("Route: New York → Los Angeles");
        expected.AppendLine("Events:");
        expected.AppendLine("[2026-02-14 20:25:00Z] RouteStarted: Starting the journey");

        result.Should().Be(expected.ToString());
    }
}
