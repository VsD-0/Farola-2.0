using Farola.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Farola.Infrastructure.Data.Configurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.ToTable("specializations", tb => tb.HasComment("Справочник специализаций"));

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id").HasComment("Идентификатор специализации");
            builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(100).HasComment("Наименование специализации");
            builder.Property(s => s.Photo).HasColumnName("photo").HasMaxLength(100).HasComment("Фото специализации");
        }
    }
}
