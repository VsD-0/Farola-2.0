using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class StatementStatusConfiguration : IEntityTypeConfiguration<StatementStatus>
    {
        public void Configure(EntityTypeBuilder<StatementStatus> builder)
        {
            builder.ToTable("statement_statuses", tb => tb.HasComment("Справочник статусов заявлений"));

            builder.HasKey(ss => ss.Id);
            builder.Property(ss => ss.Id).HasColumnName("id").HasComment("Идентификатор статуса заявления");
            builder.Property(ss => ss.Name).HasColumnName("name").HasMaxLength(40).HasComment("Наименование статуса");
        }
    }
}
