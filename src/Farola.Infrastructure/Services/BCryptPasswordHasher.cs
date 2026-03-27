using Farola.Domain.Interfaces.Services;

namespace Farola.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) => HashPassword(password);
        public bool VerifyPassword(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
    }
}
