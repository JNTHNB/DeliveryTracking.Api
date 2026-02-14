using DeliveryTracking.Application.Interfaces;
using DeliveryTracking.Domain.Interfaces;
using DeliveryTracking.Infrastructure.Events;
using DeliveryTracking.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracking.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Add services for the infrastructure layer.
    /// </summary>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddSingleton<IDomainEventDispatcher, LoggingDomainEventDispatcher>();
    }
}