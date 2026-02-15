using DeliveryTracking.Application.Behaviors;
using DeliveryTracking.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracking.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Add services for the application layer.
    /// </summary>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(DomainEventDispatchBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IDomainEventContext, Domain.Services.DomainEventContext>();
    }
}
