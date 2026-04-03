using MediatR;
namespace Farola.Application.Features.Sessions.Commands.RevokeSession
{
    public record RevokeSessionCommand(string DeviceId, string Password) : IRequest;
}
