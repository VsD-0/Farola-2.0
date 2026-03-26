using MediatR;

namespace Farola.Application.Features.Users.Commands.CreateUser
{
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
