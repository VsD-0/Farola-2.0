namespace Farola.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int StatementId { get; set; }
        public float Grade { get; set; }
        public string? Text { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual Statement Statement { get; set; } = null!;
    }
}
