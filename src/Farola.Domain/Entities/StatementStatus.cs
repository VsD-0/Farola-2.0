namespace Farola.Domain.Entities
{
    public class StatementStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Statement> Statements { get; set; } = new List<Statement>();
    }
}
