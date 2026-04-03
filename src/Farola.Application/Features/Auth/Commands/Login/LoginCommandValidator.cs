using FluentValidation;

namespace Farola.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(4).WithMessage("Password must be at least 4 characters");

            RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId is required")
            .Matches(@"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$")
            .WithMessage("DeviceId must be a valid UUID");

            RuleFor(x => x.DeviceName)
                .NotEmpty().WithMessage("DeviceName is required")
                .MaximumLength(100).WithMessage("DeviceName must not exceed 100 characters");
        }
    }
}
