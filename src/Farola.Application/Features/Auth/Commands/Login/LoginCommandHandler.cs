using Farola.Application.Common.Models;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Farola.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AccessTokenResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IDeviceFingerprintService _deviceFingerprintService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ILogger<LoginCommandHandler> logger,
            IDeviceFingerprintService deviceFingerprintService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _deviceFingerprintService = deviceFingerprintService;
        }

        public async Task<AccessTokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for email {Email}", request.Email);

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                _logger.LogWarning("Failed login for email {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty;
            
            var fingerprint = _deviceFingerprintService.ComputeFingerprint(request.DeviceId, userAgent);

            var refreshTokenEntity = new Domain.Entities.RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceId = request.DeviceId,
                DeviceName = request.DeviceName,
                IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                UserAgent = userAgent,
                DeviceFingerprint = fingerprint
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            const int maxActiveSessions = 10;
            var activeTokens = await _refreshTokenRepository.GetActiveByUserIdAsync(user.Id, cancellationToken);
            if (activeTokens.Count > maxActiveSessions)
            {
                var tokensToRevoke = activeTokens
                    .OrderBy(t => t.CreatedAt)
                    .Take(activeTokens.Count - maxActiveSessions)
                    .ToList();
                foreach (var token in tokensToRevoke)
                {
                    token.IsRevoked = true;
                    await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Revoked {Count} old sessions for user {UserId}", tokensToRevoke.Count, user.Id);
            }
            
            var isHttps = _httpContextAccessor.HttpContext?.Request.IsHttps ?? false;
            var sameSite = isHttps ? SameSiteMode.Strict : SameSiteMode.Lax;
            var secure = isHttps;

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = secure,
                SameSite = sameSite,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/"
            });

            _logger.LogInformation("User {UserId} logged in successfully", user.Id);

            return new AccessTokenResult(accessToken);
        }
    }
}
