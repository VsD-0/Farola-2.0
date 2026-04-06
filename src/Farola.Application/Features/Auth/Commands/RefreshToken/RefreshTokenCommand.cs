using Farola.Application.Common.Models;
using MediatR;

namespace Farola.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// Команда для обновления access токена с использованием refresh токена из cookie.
    /// </summary>
    public record RefreshTokenCommand() : IRequest<AccessTokenResult>;
}
