using Farola.Domain.Entities;

namespace Farola.Domain.Interfaces.Services
{
    public interface IRoleCacheService
    {
        Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Role?> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task InvalidateAsync(CancellationToken cancellationToken = default);
    }
}
