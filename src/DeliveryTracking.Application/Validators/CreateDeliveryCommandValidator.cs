using DeliveryTracking.Application.Commands;
using FluentValidation;

namespace DeliveryTracking.Application.Validators;

public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
{
    public CreateDeliveryCommandValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty();

        RuleFor(x => x.VehicleId)
            .NotEmpty();

        RuleFor(x => x.RouteId)
            .NotEmpty();
    }
}