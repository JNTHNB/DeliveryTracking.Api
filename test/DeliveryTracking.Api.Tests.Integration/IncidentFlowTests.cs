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

public class IncidentFlowTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task Delivery_IncidentFlow_ShouldBeLoggedCorrectly()
    {
        // 1. Preparation
        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        // 2. Initiation
        var createRequest = new CreateDeliveryCommand(driverId, vehicleId, routeId);
        var createResponse = await _client.PostAsJsonAsync("/deliveries", createRequest);
        var delivery = await createResponse.Content.ReadFromJsonAsync<Delivery>(_jsonOptions);
        var deliveryId = delivery!.Id;

        // 3. Start
        await _client.PostAsync($"/deliveries/{deliveryId}/start", null);

        // 4. Log Incident
        var incidentRequest = new LogEventCommand(DeliveryEventType.Incident, "Engine overheating", "Sector 7G");
        var incidentResponse = await _client.PostAsJsonAsync($"/deliveries/{deliveryId}/events", incidentRequest, _jsonOptions);
        incidentResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 5. Review
        var summaryResponse = await _client.GetAsync($"/deliveries/{deliveryId}/summary");
        var summary = await summaryResponse.Content.ReadFromJsonAsync<DeliverySummary>(_jsonOptions);
        
        summary!.Events.Should().Contain(e => 
            e.Type == DeliveryEventType.Incident && 
            e.Description == "Engine overheating" && 
            e.Location == "Sector 7G");
    }
}
