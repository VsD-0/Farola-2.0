using Farola.Domain.Entities;
using Farola.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Farola.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles", tb => tb.HasComment("Справочник ролей пользователей"));

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("id").HasComment("Идентификатор роли");
            builder.Property(r => r.Name).HasColumnName("name").HasMaxLength(20).HasComment("Наименование роли");
        }
    }
}
