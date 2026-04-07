using Farola.Domain.ValueObjects;

namespace Farola.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Email Email { get; set; } = null!;
        public PhoneNumber PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
        public string? Area { get; set; }
        public string? Information { get; set; }
        public int? SpecializationId { get; set; }
        public string? Photo { get; set; }
        public DateTime DateRegistration { get; set; }
        public string? Profession { get; set; }
        public string? Patronymic { get; set; }
        public bool IsClosed { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual Specialization? Specialization { get; set; }
        public virtual ICollection<Favorite> FavoriteClients { get; set; } = new List<Favorite>();
        public virtual ICollection<Favorite> FavoriteProfessionals { get; set; } = new List<Favorite>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Statement> StatementsAsClient { get; set; } = new List<Statement>();
        public virtual ICollection<Statement> StatementsAsProfessional { get; set; } = new List<Statement>();
    }
}
