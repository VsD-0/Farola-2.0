namespace Farola.Domain.Entities
{
    public class Statement
    {
        public int Id { get; set; }
        public int ProfessionalId { get; set; }
        public int ClientId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateExpiration { get; set; }
        public float? Grade { get; set; }
        public string? Comment { get; set; }

        public virtual User Professional { get; set; } = null!;
        public virtual User Client { get; set; } = null!;
        public virtual StatementStatus Status { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
