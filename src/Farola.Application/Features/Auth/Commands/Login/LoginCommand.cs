using Farola.Application.Common.Models;
using MediatR;

namespace Farola.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Команда для входа в систему.
    /// </summary>
    /// <param name="Email">Email пользователя (обязательный).</param>
    /// <param name="Password">Пароль (обязательный, минимум 4 символа).</param>
    /// <param name="DeviceId">Уникальный идентификатор устройства в формате UUID.</param>
    /// <param name="DeviceName">Название устройства (например, "Chrome на Windows 11").</param>
    public record LoginCommand(
        string Email,
        string Password,
        string DeviceId,
        string DeviceName
    ) : IRequest<AccessTokenResult>;
}
