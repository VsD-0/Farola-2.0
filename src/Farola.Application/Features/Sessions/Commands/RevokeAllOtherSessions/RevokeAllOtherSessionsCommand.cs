using MediatR;

namespace Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions
{
    public record RevokeAllOtherSessionsCommand(string Password) : IRequest;
}
