using Farola.Application.DTOs.Sessions.Sessions;
using MediatR;

namespace Farola.Application.Features.Sessions.Queries.GetSessions
{
    public record GetSessionsQuery(string CurrentDeviceId) : IRequest<List<SessionDto>>;
}
