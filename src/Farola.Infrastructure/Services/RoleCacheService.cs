using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Services;
using Farola.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Farola.Infrastructure.Services
{
    public class RoleCacheService : IRoleCacheService
    {
        private readonly ICacheService _cache;
        private readonly FarolaDbContext _context;
        private const string RoleKeyPrefix = "role_";

        public RoleCacheService(ICacheService cache, FarolaDbContext context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var key = $"{RoleKeyPrefix}name_{name}";
            var cached = await _cache.GetAsync<Role>(key, cancellationToken);
            if (cached != null) return cached;

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
            if (role != null)
            {
                await _cache.SetAsync(key, role, TimeSpan.FromHours(24), cancellationToken);
            }
            return role;
        }

        public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var key = $"role_id_{id}";
            var cached = await _cache.GetAsync<Role>(key, cancellationToken);
            if (cached != null) return cached;

            var role = await _context.Roles.FindAsync(new object[] { id }, cancellationToken);
            if (role != null)
            {
                await _cache.SetAsync(key, role, TimeSpan.FromHours(24), cancellationToken);
            }
            return role;
        }

        public async Task InvalidateAsync(CancellationToken cancellationToken = default)
        {
            await _cache.RemoveByPrefixAsync(RoleKeyPrefix, cancellationToken);
        }
    }
}
