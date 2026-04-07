using Farola.Domain.Entities;
using Farola.Infrastructure.Data.Configurations;
using Farola.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Farola.Infrastructure.Tests.Repositories
{
    public class RefreshTokenRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        private FarolaDbContext _context = null!;
        private RefreshTokenRepository _repository = null!;

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            var options = new DbContextOptionsBuilder<FarolaDbContext>()
                .UseNpgsql(_dbContainer.GetConnectionString())
                .Options;
            _context = new FarolaDbContext(options);
            await _context.Database.MigrateAsync();

            if (!await _context.Roles.AnyAsync())
            {
                _context.Roles.Add(new Role { Id = 1, Name = "Client" });
                await _context.SaveChangesAsync();
            }

            _repository = new RefreshTokenRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldAddRefreshToken()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                Name = "Test",
                Surname = "User",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = new RefreshToken
            {
                Token = "some_token",
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceId = "device1",
                DeviceName = "Test Device",
                IpAddress = "127.0.0.1",
                UserAgent = "TestAgent"
            };

            // Act
            await _repository.AddAsync(token, CancellationToken.None);
            await _context.SaveChangesAsync(CancellationToken.None);

            // Assert
            var saved = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "some_token");
            Assert.NotNull(saved);
            Assert.Equal(token.DeviceId, saved.DeviceId);
        }

        [Fact]
        public async Task GetByTokenAsync_ShouldReturnTokenWithUser()
        {
            // Arrange
            var user = new User
            {
                Email = "user@example.com",
                Name = "User",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = new RefreshToken
            {
                Token = "find_me",
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceId = "dev"
            };
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByTokenAsync("find_me", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(token.Token, result.Token);
            Assert.NotNull(result.User);
            Assert.Equal(user.Id, result.User.Id);
        }

        [Fact]
        public async Task RevokeAllUserTokensAsync_ShouldRevokeAllActiveTokens()
        {
            // Arrange
            var user = new User
            {
                Email = "revoke@example.com",
                Name = "Revoke",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var tokens = new[]
            {
            new RefreshToken { Token = "t1", UserId = user.Id, IsRevoked = false, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(1), DeviceId = "d1" },
            new RefreshToken { Token = "t2", UserId = user.Id, IsRevoked = false, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(1), DeviceId = "d2" },
            new RefreshToken { Token = "t3", UserId = user.Id, IsRevoked = true, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(1), DeviceId = "d3" }
        };
            _context.RefreshTokens.AddRange(tokens);
            await _context.SaveChangesAsync();

            // Act
            await _repository.RevokeAllUserTokensAsync(user.Id, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var allTokens = await _context.RefreshTokens.Where(t => t.UserId == user.Id).ToListAsync();
            Assert.True(allTokens.First(t => t.Token == "t1").IsRevoked);
            Assert.True(allTokens.First(t => t.Token == "t2").IsRevoked);
            Assert.True(allTokens.First(t => t.Token == "t3").IsRevoked); // уже был revoked
        }

        [Fact]
        public async Task GetByDeviceIdAndUserIdAsync_ShouldReturnCorrectToken()
        {
            // Arrange
            var user = new User
            {
                Email = "device@example.com",
                Name = "Device",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = new RefreshToken
            {
                Token = "dev_token",
                UserId = user.Id,
                DeviceId = "special-device",
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByDeviceIdAndUserIdAsync("special-device", user.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(token.Token, result.Token);
        }

        [Fact]
        public async Task GetActiveByUserIdAsync_ShouldReturnOnlyActiveTokens()
        {
            // Arrange
            var user = new User
            {
                Email = "active@example.com",
                Name = "Active",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var tokens = new[]
            {
            new RefreshToken { Token = "active1", UserId = user.Id, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, DeviceId = "a1" },
            new RefreshToken { Token = "active2", UserId = user.Id, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddDays(2), CreatedAt = DateTime.UtcNow, DeviceId = "a2" },
            new RefreshToken { Token = "revoked", UserId = user.Id, IsRevoked = true, ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, DeviceId = "r1" },
            new RefreshToken { Token = "expired", UserId = user.Id, IsRevoked = false, ExpiresAt = DateTime.UtcNow.AddDays(-1), CreatedAt = DateTime.UtcNow, DeviceId = "e1" }
        };
            _context.RefreshTokens.AddRange(tokens);
            await _context.SaveChangesAsync();

            // Act
            var active = await _repository.GetActiveByUserIdAsync(user.Id, CancellationToken.None);

            // Assert
            Assert.Equal(2, active.Count);
            Assert.Contains(active, t => t.Token == "active1");
            Assert.Contains(active, t => t.Token == "active2");
            Assert.DoesNotContain(active, t => t.Token == "revoked");
            Assert.DoesNotContain(active, t => t.Token == "expired");
        }
    }
}
