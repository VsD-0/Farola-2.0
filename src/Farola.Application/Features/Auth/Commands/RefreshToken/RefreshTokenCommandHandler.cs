using Farola.Application.Common.Models;
using Farola.Application.Features.Auth.Commands.Login;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Farola.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessTokenResult>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository, 
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccessTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Refresh token not found");

            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = storedToken.User;

            storedToken.IsRevoked = true;
            storedToken.LastUsedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new Domain.Entities.RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceId = storedToken.DeviceId,
                DeviceName = storedToken.DeviceName,
                IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                LastUsedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return new AccessTokenResult(newAccessToken);
        }
    }
}
