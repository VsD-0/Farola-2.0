using MediatR;

namespace Farola.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

    public record LoginResult(string AccessToken, string RefreshToken);
}
