using Farola.Application.Features.Auth.Commands.ChangePassword;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Farola.Application.Tests.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepo = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ILogger<ChangePasswordCommandHandler>> _logger = new();
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _handler = new ChangePasswordCommandHandler(
                _userRepo.Object,
                _refreshTokenRepo.Object,
                _passwordHasher.Object,
                _httpContextAccessor.Object,
                _unitOfWork.Object,
                _logger.Object);
        }

        private void SetupHttpContext(int userId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesPasswordAndRevokesTokens()
        {
            // Arrange
            var userId = 5;
            SetupHttpContext(userId);
            var command = new ChangePasswordCommand("oldPass", "newPass123");
            var user = new User { Id = userId, Password = "hashedOld" };

            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _passwordHasher.Setup(h => h.VerifyPassword(command.OldPassword, user.Password)).Returns(true);
            _passwordHasher.Setup(h => h.HashPassword(command.NewPassword)).Returns("newHashed");
            _refreshTokenRepo.Setup(r => r.RevokeAllUserTokensAsync(userId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("newHashed", user.Password);
            _userRepo.Verify(r => r.UpdateAsync(user), Times.Once);
            _refreshTokenRepo.Verify(r => r.RevokeAllUserTokensAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidOldPassword_ThrowsUnauthorized()
        {
            // Arrange
            var userId = 5;
            SetupHttpContext(userId);
            var command = new ChangePasswordCommand("wrongPass", "newPass123");
            var user = new User { Id = userId, Password = "hashedOld" };

            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _passwordHasher.Setup(h => h.VerifyPassword(command.OldPassword, user.Password)).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
            _userRepo.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
            _refreshTokenRepo.Verify(r => r.RevokeAllUserTokensAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorized()
        {
            // Arrange
            var userId = 5;
            SetupHttpContext(userId);
            var command = new ChangePasswordCommand("oldPass", "newPass123");
            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
