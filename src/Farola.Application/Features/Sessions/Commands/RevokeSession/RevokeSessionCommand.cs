using MediatR;
namespace Farola.Application.Features.Sessions.Commands.RevokeSession
{
    /// <summary>
    /// Команда для отзыва конкретной сессии по DeviceId.
    /// </summary>
    /// <param name="DeviceId">Идентификатор устройства (сессии).</param>
    /// <param name="Password">Пароль пользователя для подтверждения.</param>
    public record RevokeSessionCommand(string DeviceId, string Password) : IRequest;
}
