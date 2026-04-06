using MediatR;
using Farola.Domain.Entities;

namespace Farola.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// Запрос на получение пользователя по ID.
    /// </summary>
    /// <param name="Id">Идентификатор пользователя.</param>
    public record GetUserByIdQuery(int Id) : IRequest<User>;
}
