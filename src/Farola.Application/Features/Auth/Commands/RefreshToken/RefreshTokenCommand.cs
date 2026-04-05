using Farola.Application.Common.Models;
using MediatR;

namespace Farola.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand() : IRequest<AccessTokenResult>;
}
