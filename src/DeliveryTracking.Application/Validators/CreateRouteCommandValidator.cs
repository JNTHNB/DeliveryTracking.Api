using FluentValidation;

namespace DeliveryTracking.Application.Validators;

public class CreateRouteCommandValidator : AbstractValidator<Commands.CreateRouteCommand>
{
    public CreateRouteCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Origin)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Destination)
            .NotEmpty()
            .MaximumLength(100);

        RuleForEach(x => x.Checkpoints).ChildRules(checkpoint =>
        {
            checkpoint.RuleFor(c => c.Name)
                .NotEmpty();
            checkpoint.RuleFor(c => c.Sequence)
                .GreaterThanOrEqualTo(0);
        });
    }
}