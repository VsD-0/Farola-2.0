using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Authentication;

namespace Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsCommandHandler : IRequestHandler<RevokeAllOtherSessionsCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevokeAllOtherSessionsCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
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

            var allTokens = await _refreshTokenRepository.GetActiveByUserIdAsync(userId, cancellationToken);
            var tokensToRevoke = allTokens.Where(t => t.DeviceId != currentDeviceId).ToList();

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
            }
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
