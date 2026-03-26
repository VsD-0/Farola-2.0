using Farola.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Farola.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FarolaDbContext>
    {
        public FarolaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FarolaDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=Farola;Username=postgres;Password=FarolaPassword123");
            return new FarolaDbContext(optionsBuilder.Options);
        }
    }
}
