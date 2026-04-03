using FluentValidation;

namespace Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsCommandValidator : AbstractValidator<RevokeAllOtherSessionsCommand>
    {
        public RevokeAllOtherSessionsCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
