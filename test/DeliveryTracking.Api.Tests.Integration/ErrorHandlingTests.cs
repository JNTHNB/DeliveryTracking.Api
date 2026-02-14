using System.Net;
using System.Net.Http.Json;
using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeliveryTracking.Api.Tests.Integration;

public class ErrorHandlingTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Delivery_InvalidStateTransition_ShouldReturnBadRequest()
    {
        // 1. Preparation
        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        // 2. Initiation
        var createRequest = new CreateDeliveryCommand(driverId, vehicleId, routeId);
        var createResponse = await _client.PostAsJsonAsync("/deliveries", createRequest);
        var delivery = await createResponse.Content.ReadFromJsonAsync<Delivery>();
        var deliveryId = delivery!.Id;

        // 3. Complete (Invalid from Pending)
        var completeResponse = await _client.PostAsync($"/deliveries/{deliveryId}/complete", null);
        completeResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delivery_NonExistentDelivery_ShouldReturnBadRequest()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.GetAsync($"/deliveries/{nonExistentId}/summary");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delivery_CreateWithNonExistentRoute_ShouldReturnBadRequest()
    {
        // Arrange
        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var nonExistentRouteId = Guid.NewGuid();
        var createRequest = new CreateDeliveryCommand(driverId, vehicleId, nonExistentRouteId);

        // Act
        var response = await _client.PostAsJsonAsync("/deliveries", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
