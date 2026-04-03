using FluentValidation;

namespace Farola.Application.Features.Sessions.Commands.RevokeSession
{
    public class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
    {
        public RevokeSessionCommandValidator()
        {
            RuleFor(x => x.DeviceId)
                .NotEmpty().WithMessage("DeviceId is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
