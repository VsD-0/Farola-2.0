namespace Farola.Domain.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string plainPassword, string hashedPassword);
    }
}
