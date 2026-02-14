using DeliveryTracking.Application.Commands;
using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Application.Queries;
using DeliveryTracking.Domain.Aggregates;
using MediatR;
using Route = DeliveryTracking.Domain.Aggregates.Route;

namespace DeliveryTracking.Api;

public static class EndpointDefinitions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        MapDriverEndpoints(app);
        MapVehicleEndpoints(app);
        MapRouteEndpoints(app);
        MapDeliveryEndpoints(app);
    }

    private static void MapDriverEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/drivers").WithTags("Drivers");

        group.MapGet("/", async (IDriverRepository repo) => await repo.List());
        group.MapPost("/", async (IDriverRepository repo, Driver driver) =>
        {
            driver.Id = Guid.NewGuid();
            await repo.Add(driver);
            return Results.Created($"/drivers/{driver.Id}", driver);
        });
    }

    private static void MapVehicleEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IVehicleRepository repo) => await repo.List());
        group.MapPost("/", async (IVehicleRepository repo, Vehicle vehicle) =>
        {
            vehicle.Id = Guid.NewGuid();
            await repo.Add(vehicle);
            return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
        });
    }

    private static void MapRouteEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/routes").WithTags("Routes");

        group.MapGet("/", async (IRouteRepository repo) => await repo.List());
        group.MapPost("/", async (IRouteRepository repo, Route route) =>
        {
            route.Id = Guid.NewGuid();
            await repo.Add(route);
            return Results.Created($"/routes/{route.Id}", route);
        });
    }

    private static void MapDeliveryEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/deliveries").WithTags("Deliveries");

        group.MapPost("/", async (IMediator mediator, CreateDeliveryCommand command) =>
        {
            try
            {
                var delivery = await mediator.Send(command);
                var deliveryId = delivery.Id.ToString();
                return Results.Created($"/deliveries/{deliveryId}", delivery);
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{id:guid}/start", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var command = new StartDeliveryCommand(id);
                await mediator.Send(command);
                return Results.Ok();
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{id:guid}/events", async (Guid id, IMediator mediator, LogEventCommand command) =>
        {
            try
            {
                command.DeliveryId = id;
                await mediator.Send(command);
                return Results.Ok();
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{id:guid}/complete", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var command = new CompleteDeliveryCommand(id);
                await mediator.Send(command);
                return Results.Ok();
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapGet("/{id:guid}/summary", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var query = new GetDeliverySummaryQuery(id);
                var summary = await mediator.Send(query);
                return Results.Ok(summary);
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapGet("/active", async (IMediator mediator) =>
        {
            var query = new GetActiveDeliveriesQuery();
            var deliveries = await mediator.Send(query);
            return Results.Ok(deliveries);
        });
    }
}



