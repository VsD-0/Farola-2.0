using MediatR;

namespace Farola.Application.Features.Auth.Commands.ChangePassword
{
    /// <summary>
    /// Команда для смены пароля пользователя.
    /// </summary>
    /// <param name="OldPassword">Текущий пароль.</param>
    /// <param name="NewPassword">Новый пароль.</param>
    public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest;
}
