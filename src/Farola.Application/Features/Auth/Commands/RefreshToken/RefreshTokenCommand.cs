using Farola.Application.Features.Auth.Commands.Login;
using MediatR;

namespace Farola.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResult>;
}
