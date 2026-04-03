using FluentValidation;

namespace Farola.Application.Features.Sessions.Queries.GetSessions
{
    public class GetSessionsQueryValidator : AbstractValidator<GetSessionsQuery>
    {
        public GetSessionsQueryValidator()
        {
            RuleFor(x => x.CurrentDeviceId)
                .NotEmpty().WithMessage("CurrentDeviceId is required");
        }
    }
}
