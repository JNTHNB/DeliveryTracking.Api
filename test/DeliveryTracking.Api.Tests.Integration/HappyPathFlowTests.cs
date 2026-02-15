using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeliveryTracking.Application.Commands;
using DeliveryTracking.Application.Models;
using DeliveryTracking.Domain.Aggregates;
using DeliveryTracking.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeliveryTracking.Api.Tests.Integration;

public class HappyPathFlowTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task Delivery_FullBusinessFlow_ShouldSucceed()
    {
        // 1. Preparation (Already handled by DataSeeder in Program.cs)
        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        // 2. Initiation
        var createRequest = new CreateDeliveryCommand(driverId, vehicleId, routeId);
        var createResponse = await _client.PostAsJsonAsync("/deliveries", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var rawContent = await createResponse.Content.ReadAsStringAsync();
        rawContent.Should().Contain("\"status\":\"Pending\"");
        
        var delivery = await createResponse.Content.ReadFromJsonAsync<Delivery>(_jsonOptions);
        delivery.Should().NotBeNull();
        var deliveryId = delivery!.Id;
        delivery.Status.Should().Be(DeliveryStatus.Pending);

        // 3. Start
        var startResponse = await _client.PostAsync($"/deliveries/{deliveryId}/start", null);
        startResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 4. Tracking
        var logEventRequest = new LogEventCommand(DeliveryEventType.CheckpointReached, "Arrived at Asteroid Belt", "Asteroid Belt");
        var logResponse = await _client.PostAsJsonAsync($"/deliveries/{deliveryId}/events", logEventRequest, _jsonOptions);
        logResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 5. Completion
        var completeResponse = await _client.PostAsync($"/deliveries/{deliveryId}/complete", null);
        completeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 6. Review
        var summaryResponse = await _client.GetAsync($"/deliveries/{deliveryId}/summary");
        summaryResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var summary = await summaryResponse.Content.ReadFromJsonAsync<DeliverySummary>(_jsonOptions);
        summary.Should().NotBeNull();
        summary!.DeliveryId.Should().Be(deliveryId);
        summary.Status.Should().Be(DeliveryStatus.Completed);
        summary.Events.Should().HaveCount(3); // RouteStarted, CheckpointReached, DeliveryCompleted
        summary.Events.Should().Contain(e => e.Type == DeliveryEventType.CheckpointReached);
    }
}
