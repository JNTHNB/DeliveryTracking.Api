using DeliveryTracking.Application.Commands;
using DeliveryTracking.Application.Exceptions;
using DeliveryTracking.Application.Models;
using DeliveryTracking.Application.Queries;
using DeliveryTracking.Domain.Exceptions;
using FluentValidation;
using MediatR;

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

        group.MapGet("/", async (IMediator mediator) =>
        {
            var drivers = await mediator.Send(new GetDriversQuery());
            return Results.Ok(drivers);
        });
        group.MapPost("/", async (IMediator mediator, CreateDriverCommand command) =>
        {
            try
            {
                var driver = await mediator.Send(command);
                return Results.Created($"/drivers/{driver.Id}", driver);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));
            }
        });
    }

    private static void MapVehicleEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var vehicles = await mediator.Send(new GetVehiclesQuery());
            return Results.Ok(vehicles);
        });
        group.MapPost("/", async (IMediator mediator, CreateVehicleCommand command) =>
        {
            try
            {
                var vehicle = await mediator.Send(command);
                return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));
            }
        });
    }

    private static void MapRouteEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/routes").WithTags("Routes");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var routes = await mediator.Send(new GetRoutesQuery());
            return Results.Ok(routes);
        });
        group.MapPost("/", async (IMediator mediator, CreateRouteCommand command) =>
        {
            try
            {
                var route = await mediator.Send(command);
                return Results.Created($"/routes/{route.Id}", route);
            }
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));
            }
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
            catch (ValidationException ex)
            {
                return Results.ValidationProblem(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));
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
            catch (DomainException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPost("/{id:guid}/events", async (Guid id, IMediator mediator, LogEventRequest request) =>
        {
            try
            {
                var command = new LogEventCommand(id, request.Type, request.Description, request.Location);
                await mediator.Send(command);
                return Results.Ok();
            }
            catch (IdNotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (DomainException ex)
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
            catch (DomainException ex)
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



