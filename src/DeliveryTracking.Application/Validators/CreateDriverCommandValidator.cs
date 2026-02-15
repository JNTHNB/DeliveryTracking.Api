using DeliveryTracking.Application.Commands;
using FluentValidation;

namespace DeliveryTracking.Application.Validators;

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
