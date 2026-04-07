using Farola.Domain.Entities;
using Farola.Infrastructure.Data.Configurations;
using Farola.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Farola.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        private FarolaDbContext _context = null!;
        private UserRepository _repository = null!;

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
                await _context.Roles.AddAsync(new Role { Id = 1, Name = "Client" });
                await _context.SaveChangesAsync();
            }

            _repository = new UserRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldSaveUser()
        {
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
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            var saved = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(saved);
            Assert.Equal("Test", saved.Name);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var user = new User
            {
                Email = "unique@example.com",
                Name = "Email",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var found = await _repository.GetByEmailAsync("unique@example.com");

            // Assert
            Assert.NotNull(found);
            Assert.Equal(user.Email, found.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenNotExists()
        {
            var found = await _repository.GetByEmailAsync("nonexistent@example.com");
            Assert.Null(found);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenExists()
        {
            var user = new User
            {
                Email = "exists@example.com",
                Name = "Exists",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            var exists = await _repository.EmailExistsAsync("exists@example.com");
            Assert.True(exists);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenNotExists()
        {
            var exists = await _repository.EmailExistsAsync("notexists@example.com");
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserWithoutSaving()
        {
            // Arrange
            var user = new User
            {
                Email = "update@example.com",
                Name = "Original",
                Surname = "Test",
                PhoneNumber = "+1234567890",
                RoleId = 1,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            _context.Entry(user).State = EntityState.Detached;

            var updatedUser = new User { Id = user.Id, Name = "Updated", Surname = "Test", Email = user.Email, PhoneNumber = user.PhoneNumber, RoleId = user.RoleId, DateRegistration = user.DateRegistration, IsClosed = user.IsClosed };
            // Act
            await _repository.UpdateAsync(updatedUser);
            var entry = _context.Entry(updatedUser);
            Assert.Equal(EntityState.Modified, entry.State);
        }
    }
}
