namespace Farola.Domain.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public int ProfessionalId { get; set; }
        public int ClientId { get; set; }

        public virtual User Professional { get; set; } = null!;
        public virtual User Client { get; set; } = null!;
    }
}
