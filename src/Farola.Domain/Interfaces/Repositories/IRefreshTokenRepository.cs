using Farola.Domain.Entities;

namespace Farola.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task RevokeTokenAsync(RefreshToken refreshToken);
        Task RevokeAllUserTokensAsync(int userId);
    }
}
