using MediatR;
using Farola.Domain.Entities;

namespace Farola.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<User>;
}
