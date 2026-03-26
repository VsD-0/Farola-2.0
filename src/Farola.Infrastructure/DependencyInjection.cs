using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Farola.Infrastructure.Data.Configurations;

namespace Farola.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FarolaDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Здесь позже добавите регистрацию репозиториев: services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
