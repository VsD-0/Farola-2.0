using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class RefreshTokenTests
    {
        [Fact]
        public void RefreshToken_CanBeCreated()
        {
            var token = new RefreshToken
            {
                Id = 1,
                UserId = 5,
                Token = "abc123",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceId = "device-1",
                DeviceName = "MyDevice",
                IpAddress = "127.0.0.1",
                UserAgent = "TestAgent",
                LastUsedAt = DateTime.UtcNow,
                DeviceFingerprint = "test-fingerprint"
            };

            Assert.Equal(1, token.Id);
            Assert.Equal("abc123", token.Token);
            Assert.False(token.IsRevoked);
            Assert.Equal("device-1", token.DeviceId);
            Assert.Equal("MyDevice", token.DeviceName);
            Assert.NotNull(token.LastUsedAt);
            Assert.Equal("test-fingerprint", token.DeviceFingerprint);
        }

        [Fact]
        public void RefreshToken_LastUsedAt_CanBeNull()
        {
            var token = new RefreshToken
            {
                Token = "token",
                UserId = 1,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                DeviceId = "d",
                DeviceName = "n",
                DeviceFingerprint = null
            };
            Assert.Null(token.LastUsedAt);
            Assert.Null(token.DeviceFingerprint);
        }
    }
}
