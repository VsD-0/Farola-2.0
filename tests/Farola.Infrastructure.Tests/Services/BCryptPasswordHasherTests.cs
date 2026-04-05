using Farola.Infrastructure.Services;

namespace Farola.Infrastructure.Tests.Services
{
    public class BCryptPasswordHasherTests
    {
        private readonly BCryptPasswordHasher _hasher = new();

        [Fact]
        public void HashPassword_ShouldReturnHash()
        {
            var hash = _hasher.HashPassword("password123");
            Assert.NotNull(hash);
            Assert.StartsWith("$2a$", hash);
        }

        [Fact]
        public void VerifyPassword_ValidPassword_ReturnsTrue()
        {
            var hash = _hasher.HashPassword("correct");
            Assert.True(_hasher.VerifyPassword("correct", hash));
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ReturnsFalse()
        {
            var hash = _hasher.HashPassword("correct");
            Assert.False(_hasher.VerifyPassword("wrong", hash));
        }
    }
}
