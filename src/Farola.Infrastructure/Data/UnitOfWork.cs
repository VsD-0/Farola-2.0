using Farola.Domain.Interfaces;
using Farola.Infrastructure.Data.Configurations;

namespace Farola.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FarolaDbContext _context; 
        public UnitOfWork(FarolaDbContext context)
        { 
            _context = context; 
        }
        public async Task<int> SaveChangesAsync (CancellationToken cancellationToken =default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
