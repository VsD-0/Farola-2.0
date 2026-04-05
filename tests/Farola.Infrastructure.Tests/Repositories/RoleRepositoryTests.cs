using Farola.Domain.Entities;
using Farola.Infrastructure.Data.Configurations;
using Farola.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Farola.Infrastructure.Tests.Repositories
{
    public class RoleRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        private FarolaDbContext _context = null!;
        private RoleRepository _repository = null!;

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
                _context.Roles.AddRange(
                    new Role { Name = "Client" },
                    new Role { Name = "Professional" },
                    new Role { Name = "Admin" }
                );
                await _context.SaveChangesAsync();
            }

            _repository = new RoleRepository(_context);
        }

        public async Task DisposeAsync() => await _dbContainer.DisposeAsync();

        [Fact]
        public async Task GetByNameAsync_ShouldReturnRole_WhenExists()
        {
            // Act
            var role = await _repository.GetByNameAsync("Client");

            // Assert
            Assert.NotNull(role);
            Assert.Equal("Client", role.Name);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNull_WhenNotExists()
        {
            var role = await _repository.GetByNameAsync("NonExistentRole");
            Assert.Null(role);
        }
    }
}
