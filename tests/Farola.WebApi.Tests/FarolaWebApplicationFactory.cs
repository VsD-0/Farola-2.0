using Farola.Domain.Entities;
using Farola.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Testcontainers.PostgreSql;

namespace Farola.WebApi.Tests
{
    public class FarolaWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

#pragma warning disable CS0809
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FarolaDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<FarolaDbContext>(options =>
                    options.UseNpgsql(_dbContainer.GetConnectionString()));
            });
        }
#pragma warning restore CS0809

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FarolaDbContext>();
            await db.Database.MigrateAsync();
            if (!await db.Roles.AnyAsync())
            {
                db.Roles.Add(new Role { Id = 1, Name = "Client" });
                await db.SaveChangesAsync();
            }
        }

#pragma warning disable CS0114
#pragma warning disable CS0618
        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
#pragma warning restore CS0809
#pragma warning restore CS0618
    }
}
