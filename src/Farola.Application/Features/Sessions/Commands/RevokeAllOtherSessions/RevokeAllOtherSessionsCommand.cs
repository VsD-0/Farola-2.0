using MediatR;

namespace Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions
{
    /// <summary>
    /// Команда для отзыва всех сессий, кроме текущей.
    /// </summary>
    /// <param name="Password">Пароль пользователя для подтверждения.</param>
    public record RevokeAllOtherSessionsCommand(string Password) : IRequest;
}
