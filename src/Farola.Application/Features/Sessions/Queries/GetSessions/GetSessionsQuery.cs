using Farola.Application.DTOs.Sessions;
using MediatR;

namespace Farola.Application.Features.Sessions.Queries.GetSessions
{
    /// <summary>
    /// Запрос на получение списка активных сессий.
    /// </summary>
    /// <param name="CurrentDeviceId">Идентификатор текущего устройства (передаётся в заголовке X-Device-Id).</param>
    public record GetSessionsQuery(string CurrentDeviceId) : IRequest<List<SessionDto>>;
}
