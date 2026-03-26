namespace Farola.Domain.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
