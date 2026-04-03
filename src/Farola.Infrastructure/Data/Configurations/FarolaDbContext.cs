using Microsoft.EntityFrameworkCore;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class FarolaDbContext : DbContext
    {
        public FarolaDbContext(DbContextOptions<FarolaDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Specialization> Specializations => Set<Specialization>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Statement> Statements => Set<Statement>();
        public DbSet<StatementStatus> StatementStatuses => Set<StatementStatus>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new StatementStatusConfiguration());
            modelBuilder.ApplyConfiguration(new StatementConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        }
    }
}
