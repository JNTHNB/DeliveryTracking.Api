using DeliveryTracking.Domain.ValueObjects;

namespace DeliveryTracking.Application.Models;

public record LogEventRequest(DeliveryEventType Type, string Description, string? Location);
