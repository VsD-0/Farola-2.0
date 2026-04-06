using Farola.Application.Features.Auth.Commands.Login;
using Farola.Application.Features.Sessions.Queries.GetSessions;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Farola.Application.Tests.Features.Auth.Commands.Login
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<ITokenService> _tokenService = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepo = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly LoginCommandHandler _handler;
        private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock = new();
        private readonly Mock<IDeviceFingerprintService> _fingerprintServiceMock = new();

        public LoginCommandHandlerTests()
        {
            _handler = new LoginCommandHandler(
                _userRepo.Object,
                _tokenService.Object,
                _refreshTokenRepo.Object,
                _hasher.Object,
                _httpContextAccessor.Object,
                _unitOfWork.Object,
                _loggerMock.Object,
                _fingerprintServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsAccessTokenAndSetsCookie()
        {
            // Arrange
            var user = new User { Id = 5, Email = "test@example.com", Password = "hashed" };
            var command = new LoginCommand("test@example.com", "password123", "device-123", "Test Device");

            _userRepo.Setup(r => r.GetByEmailAsync(command.Email))
                .ReturnsAsync(user);
            _hasher.Setup(h => h.VerifyPassword(command.Password, user.Password)).Returns(true);
            _tokenService.Setup(t => t.GenerateAccessToken(user)).Returns("access_token");
            _tokenService.Setup(t => t.GenerateRefreshToken()).Returns("refresh_token");

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["User-Agent"] = "TestAgent";
            httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("access_token", result.AccessToken);

            _refreshTokenRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
            Assert.Contains("HttpOnly", setCookieHeader, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorized()
        {
            var command = new LoginCommand("wrong@example.com", "pass", "dev", "name");
            _userRepo.Setup(r => r.GetByEmailAsync(command.Email))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _refreshTokenRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorized()
        {
            var user = new User { Id = 5, Email = "test@example.com", Password = "hashed" };
            var command = new LoginCommand("test@example.com", "wrong", "dev", "name");
            _userRepo.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);
            _hasher.Setup(h => h.VerifyPassword(command.Password, user.Password)).Returns(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _refreshTokenRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
