using Farola.Domain.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Farola.Application.DTOs.Sessions;

namespace Farola.Application.Features.Sessions.Queries.GetSessions
{
    public class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, List<SessionDto>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetSessionsQueryHandler> _logger;

        public GetSessionsQueryHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetSessionsQueryHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<SessionDto>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated");

            _logger.LogDebug("User {UserId} requested list of active sessions", userId);

            var tokens = await _refreshTokenRepository.GetActiveByUserIdAsync(userId, cancellationToken);
            var sorted = tokens.OrderByDescending(t => t.LastUsedAt ?? t.CreatedAt).ToList();

            return sorted.Select(t => new SessionDto(
                t.Id,
                t.DeviceId,
                t.DeviceName ?? "Unknown device",
                t.CreatedAt,
                t.ExpiresAt,
                t.IpAddress ?? "",
                t.UserAgent ?? "",
                IsCurrentDevice: t.DeviceId == request.CurrentDeviceId   // точное сравнение
            )).ToList();
        }
    }
}
