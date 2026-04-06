using Farola.Application.Features.Auth.Commands.Login;
using Farola.Application.Features.Auth.Commands.RefreshToken;
using Farola.Application.Features.Sessions.Commands.RevokeSession;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Farola.Application.Tests.Features.Sessions.Commands.RevokeSession
{
    public class RefreshTokenCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepo = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly RefreshTokenCommandHandler _handler;
        private readonly Mock<ILogger<RefreshTokenCommandHandler>> _loggerMock = new();

        public RefreshTokenCommandHandlerTests()
        {
            _handler = new RefreshTokenCommandHandler(
                _refreshTokenRepo.Object,
                _tokenService.Object,
                _httpContextAccessor.Object,
                _unitOfWork.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRefreshToken_ReturnsNewTokens()
        {
            // Arrange
            var refreshTokenValue = "valid_refresh_token";
            var command = new RefreshTokenCommand();

            var user = new User { Id = 5, Email = "user@example.com" };
            var storedToken = new RefreshToken
            {
                Token = refreshTokenValue,
                User = user,
                UserId = user.Id,
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                DeviceId = "device1",
                DeviceName = "Test Device",
                IpAddress = "127.0.0.1",
                UserAgent = "TestAgent"
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["User-Agent"] = "TestAgent";
            httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            httpContext.Request.Cookies = new MockRequestCookieCollection(new Dictionary<string, string>
            {
                ["refreshToken"] = refreshTokenValue
            });
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            _refreshTokenRepo.Setup(r => r.GetByTokenAsync(refreshTokenValue, It.IsAny<CancellationToken>()))
                .ReturnsAsync(storedToken);
            _tokenService.Setup(t => t.GenerateAccessToken(user)).Returns("new_access_token");
            _tokenService.Setup(t => t.GenerateRefreshToken()).Returns("new_refresh_token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("new_access_token", result.AccessToken);

            Assert.True(storedToken.IsRevoked);
            Assert.NotNull(storedToken.LastUsedAt);
            _refreshTokenRepo.Verify(r => r.UpdateAsync(storedToken, It.IsAny<CancellationToken>()), Times.Once);

            _refreshTokenRepo.Verify(r => r.AddAsync(It.Is<RefreshToken>(t =>
                t.Token == "new_refresh_token" &&
                t.UserId == user.Id &&
                !t.IsRevoked &&
                t.DeviceId == storedToken.DeviceId
            ), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
        }

        [Fact]
        public async Task Handle_MissingRefreshTokenCookie_ThrowsUnauthorized()
        {
            var command = new RefreshTokenCommand();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Cookies = new MockRequestCookieCollection(new Dictionary<string, string>());
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _refreshTokenRepo.Verify(r => r.GetByTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidOrExpiredToken_ThrowsUnauthorized()
        {
            var refreshTokenValue = "expired_token";
            var command = new RefreshTokenCommand();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Cookies = new MockRequestCookieCollection(new Dictionary<string, string>
            {
                ["refreshToken"] = refreshTokenValue
            });
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            _refreshTokenRepo.Setup(r => r.GetByTokenAsync(refreshTokenValue, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken?)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        private class MockRequestCookieCollection : Dictionary<string, string>, IRequestCookieCollection
        {
            public MockRequestCookieCollection(IDictionary<string, string> dict) : base(dict) { }
            public new ICollection<string> Keys => base.Keys;
        }
    }
}
