using Farola.Domain.Entities;
using Farola.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Farola.Infrastructure.Tests.Services
{
    public class JwtTokenServiceTests
    {
        private readonly JwtSettings _settings = new JwtSettings
        {
            Secret = "my_super_secret_key_which_is_at_least_32_characters_long",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 15
        };
        private readonly JwtTokenService _service;

        public JwtTokenServiceTests()
        {
            var options = Options.Create(_settings);
            _service = new JwtTokenService(options);
        }

        [Fact]
        public void GenerateAccessToken_ShouldReturnValidJwt()
        {
            var user = new User { Id = 5, Email = "test@example.com", Role = new Role { Id = 1, Name = "Client" } };
            var token = _service.GenerateAccessToken(user);
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnNonEmptyString()
        {
            var token = _service.GenerateRefreshToken();
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }
    }
}
