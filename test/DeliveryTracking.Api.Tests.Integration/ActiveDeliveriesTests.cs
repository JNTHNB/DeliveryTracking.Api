using System.Net;
using System.Net.Http.Json;
using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeliveryTracking.Api.Tests.Integration;

public class ActiveDeliveriesTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetActiveDeliveries_ShouldReturnOnlyPendingAndInProgressDeliveries()
    {
        // Arrange
        var driverId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var vehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var routeId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        // Create 3 deliveries: one will stay pending, one will be in progress, one will be completed
        var createRequest = new CreateDeliveryCommand(driverId, vehicleId, routeId);
        
        // 1. Pending
        var response1 = await _client.PostAsJsonAsync("/deliveries", createRequest);
        var delivery1 = await response1.Content.ReadFromJsonAsync<Delivery>();

        // 2. In Progress
        var response2 = await _client.PostAsJsonAsync("/deliveries", createRequest);
        var delivery2 = await response2.Content.ReadFromJsonAsync<Delivery>();
        await _client.PostAsync($"/deliveries/{delivery2!.Id}/start", null);

        // 3. Completed
        var response3 = await _client.PostAsJsonAsync("/deliveries", createRequest);
        var delivery3 = await response3.Content.ReadFromJsonAsync<Delivery>();
        await _client.PostAsync($"/deliveries/{delivery3!.Id}/start", null);
        await _client.PostAsync($"/deliveries/{delivery3!.Id}/complete", null);

        // Act
        var activeResponse = await _client.GetAsync("/deliveries/active");

        // Assert
        activeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var activeDeliveries = await activeResponse.Content.ReadFromJsonAsync<List<Delivery>>();
        
        activeDeliveries.Should().NotBeNull();
        activeDeliveries.Should().Contain(d => d.Id == delivery1!.Id);
        activeDeliveries.Should().Contain(d => d.Id == delivery2!.Id);
        activeDeliveries.Should().NotContain(d => d.Id == delivery3!.Id);
        
        activeDeliveries!.All(d => d.Status is DeliveryStatus.Pending or DeliveryStatus.InProgress).Should().BeTrue();
    }
}
