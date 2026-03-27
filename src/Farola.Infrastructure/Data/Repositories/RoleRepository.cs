using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;
using Farola.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Farola.Infrastructure.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly FarolaDbContext _context;

        public RoleRepository(FarolaDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }
    }
}
