using Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Farola.Domain.Interfaces.Services;
using Moq;
using System.Security.Claims;

namespace Farola.Application.Tests.Features.Sessions.Commands.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsCommandHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepo = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly RevokeAllOtherSessionsCommandHandler _handler;

        public RevokeAllOtherSessionsCommandHandlerTests()
        {
            _handler = new RevokeAllOtherSessionsCommandHandler(
                _refreshTokenRepo.Object,
                _userRepo.Object,
                _passwordHasher.Object,
                _httpContextAccessor.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_RevokesAllOtherSessions()
        {
            // Arrange
            var userId = 5;
            var currentDeviceId = "current-device";
            var password = "pass";
            var command = new RevokeAllOtherSessionsCommand(password);

            var user = new User { Id = userId, Password = "hashed" };
            var tokens = new List<RefreshToken>
        {
            new RefreshToken { DeviceId = currentDeviceId, IsRevoked = false, UserId = userId },
            new RefreshToken { DeviceId = "other1", IsRevoked = false, UserId = userId },
            new RefreshToken { DeviceId = "other2", IsRevoked = false, UserId = userId }
        };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            httpContext.Items["DeviceId"] = currentDeviceId;
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _passwordHasher.Setup(h => h.VerifyPassword(password, user.Password)).Returns(true);
            _refreshTokenRepo.Setup(r => r.GetActiveByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokens);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var token in tokens)
            {
                if (token.DeviceId != currentDeviceId)
                    Assert.True(token.IsRevoked);
                else
                    Assert.False(token.IsRevoked);
            }
            _refreshTokenRepo.Verify(r => r.UpdateAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
