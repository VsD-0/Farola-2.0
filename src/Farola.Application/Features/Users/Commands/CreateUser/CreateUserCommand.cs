using MediatR;

namespace Farola.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Команда для создания нового пользователя.
    /// </summary>
    /// <param name="Email">Email пользователя.</param>
    /// <param name="Password">Пароль.</param>
    /// <param name="Surname">Фамилия.</param>
    /// <param name="Name">Имя.</param>
    /// <param name="PhoneNumber">Номер телефона.</param>
    /// <param name="RoleId">ID роли (1 – Client, 2 – Professional, 3 – Admin).</param>
    /// <param name="Patronymic">Отчество (опционально).</param>
    /// <param name="Profession">Профессия (для специалиста).</param>
    /// <param name="Area">Регион/город.</param>
    /// <param name="Information">Дополнительная информация.</param>
    /// <param name="SpecializationId">ID специализации (для специалиста).</param>
    /// <param name="Photo">Имя файла фото.</param>
    public record CreateUserCommand(
    string Email,
    string Password,
    string Surname,
    string Name,
    string PhoneNumber,
    int RoleId,
    string? Patronymic = null,
    string? Profession = null,
    string? Area = null,
    string? Information = null,
    int? SpecializationId = null,
    string? Photo = null
) : IRequest<int>;
}
