using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeliveryTracking.Application.Commands;
using DeliveryTracking.Domain.Aggregates;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeliveryTracking.Api.Tests.Integration;

public class EndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task Driver_Endpoints_ShouldWork()
    {
        // Create Driver
        var createRequest = new CreateDriverCommand("Test Driver");
        var createResponse = await _client.PostAsJsonAsync("/drivers", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var driver = await createResponse.Content.ReadFromJsonAsync<Driver>(_jsonOptions);
        driver!.Name.Should().Be("Test Driver");

        // List Drivers
        var listResponse = await _client.GetAsync("/drivers");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var drivers = await listResponse.Content.ReadFromJsonAsync<IEnumerable<Driver>>(_jsonOptions);
        drivers.Should().Contain(d => d.Id == driver.Id);
    }

    [Fact]
    public async Task Vehicle_Endpoints_ShouldWork()
    {
        // Create Vehicle
        var createRequest = new CreateVehicleCommand("Test Vehicle", VehicleType.RocketVan);
        var createResponse = await _client.PostAsJsonAsync("/vehicles", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var vehicle = await createResponse.Content.ReadFromJsonAsync<Vehicle>(_jsonOptions);
        vehicle!.Name.Should().Be("Test Vehicle");
        vehicle.Type.Should().Be(VehicleType.RocketVan);

        // List Vehicles
        var listResponse = await _client.GetAsync("/vehicles");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var vehicles = await listResponse.Content.ReadFromJsonAsync<IEnumerable<Vehicle>>(_jsonOptions);
        vehicles.Should().Contain(v => v.Id == vehicle.Id);
    }

    [Fact]
    public async Task Route_Endpoints_ShouldWork()
    {
        // Create Route
        var createRequest = new CreateRouteCommand("Test Route", "Earth", "Mars", []);
        var createResponse = await _client.PostAsJsonAsync("/routes", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var route = await createResponse.Content.ReadFromJsonAsync<Route>(_jsonOptions);
        route!.Name.Should().Be("Test Route");

        // List Routes
        var listResponse = await _client.GetAsync("/routes");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var routes = await listResponse.Content.ReadFromJsonAsync<IEnumerable<Route>>(_jsonOptions);
        routes.Should().Contain(r => r.Id == route.Id);
    }

    [Fact]
    public async Task Driver_Create_Validation_ShouldFail_WhenNameEmpty()
    {
        var createRequest = new CreateDriverCommand("");
        var createResponse = await _client.PostAsJsonAsync("/drivers", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
