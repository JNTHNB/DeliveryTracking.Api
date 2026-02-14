using DeliveryTracking.Application.Behaviors;
using DeliveryTracking.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracking.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Add services for the application layer.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(DomainEventDispatchBehavior<,>));
        });

        services.AddScoped<IDomainEventContext, DeliveryTracking.Domain.Services.DomainEventContext>();

        return services;
    }
}
