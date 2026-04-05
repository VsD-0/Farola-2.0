using Farola.Application.Features.Sessions.Queries.GetSessions;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Farola.Application.Tests.Features.Sessions.Queries.GetSessions
{
    public class GetSessionsQueryHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepo = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly GetSessionsQueryHandler _handler;

        public GetSessionsQueryHandlerTests()
        {
            _handler = new GetSessionsQueryHandler(_refreshTokenRepo.Object, _httpContextAccessor.Object);
        }

        [Fact]
        public async Task Handle_ReturnsSessions_WithCurrentDeviceFlag()
        {
            // Arrange
            var userId = 5;
            var currentDeviceId = "device-current";
            var query = new GetSessionsQuery(currentDeviceId);

            var tokens = new List<RefreshToken>
        {
            new RefreshToken { Id = 1, DeviceId = currentDeviceId, DeviceName = "Current", CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(1), IpAddress = "1.1.1.1", UserAgent = "Agent1", LastUsedAt = DateTime.UtcNow },
            new RefreshToken { Id = 2, DeviceId = "other", DeviceName = "Other", CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(2), IpAddress = "2.2.2.2", UserAgent = "Agent2", LastUsedAt = DateTime.UtcNow }
        };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

            _refreshTokenRepo.Setup(r => r.GetActiveByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokens);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            var current = result.First(s => s.DeviceId == currentDeviceId);
            Assert.True(current.IsCurrentDevice);
            var other = result.First(s => s.DeviceId == "other");
            Assert.False(other.IsCurrentDevice);
        }
    }
}
