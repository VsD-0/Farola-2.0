using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using System.Security.Claims;

namespace Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsCommandHandler : IRequestHandler<RevokeAllOtherSessionsCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokeAllOtherSessionsCommandHandler> _logger;

        public RevokeAllOtherSessionsCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ILogger<RevokeAllOtherSessionsCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(RevokeAllOtherSessionsCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid password");

            var currentDeviceId = _httpContextAccessor.HttpContext?.Items["DeviceId"]?.ToString();
            if (string.IsNullOrEmpty(currentDeviceId))
                throw new InvalidOperationException("DeviceId not found in context");

            _logger.LogInformation("User {UserId} requested to revoke all other sessions", userId);

            var allTokens = await _refreshTokenRepository.GetActiveByUserIdAsync(userId, cancellationToken);
            var tokensToRevoke = allTokens.Where(t => t.DeviceId != currentDeviceId).ToList();

            if (tokensToRevoke.Any())
            {
                foreach (var token in tokensToRevoke)
                {
                    token.IsRevoked = true;
                    await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Revoked {Count} sessions for user {UserId}", tokensToRevoke.Count, userId);
            }
            else
            {
                _logger.LogInformation("No other sessions to revoke for user {UserId}", userId);
            }
        }
    }
}
