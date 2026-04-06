using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;
using System.Security.Claims;

namespace Farola.Application.Features.Sessions.Commands.RevokeSession
{
    public class RevokeSessionCommandHandler : IRequestHandler<RevokeSessionCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokeSessionCommandHandler> _logger;

        public RevokeSessionCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ILogger<RevokeSessionCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(RevokeSessionCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid password");

            _logger.LogInformation("User {UserId} requested to revoke session with DeviceId {DeviceId}", userId, request.DeviceId);

            var token = await _refreshTokenRepository.GetByDeviceIdAndUserIdAsync(request.DeviceId, userId, cancellationToken);
            if (token != null && !token.IsRevoked)
            {
                token.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Revoked session with DeviceId {DeviceId} for user {UserId}", request.DeviceId, userId);
            }
            else
            {
                _logger.LogWarning("Session with DeviceId {DeviceId} for user {UserId} not found or already revoked", request.DeviceId, userId);
            }
        }
    }
}
