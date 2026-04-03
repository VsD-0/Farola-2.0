using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("favorites", tb => tb.HasComment("Таблица избранных специалистов"));

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).HasColumnName("id").HasComment("Идентификатор");
            builder.Property(f => f.ProfessionalId).HasColumnName("professional_id").HasComment("Номер специалиста");
            builder.Property(f => f.ClientId).HasColumnName("client_id").HasComment("Номер клиента");

            builder.HasIndex(f => new { f.ProfessionalId, f.ClientId })
                .IsUnique()
                .HasDatabaseName("ix_favorites_professional_client");

            builder.HasOne(f => f.Professional)
                .WithMany(u => u.FavoriteProfessionals)
                .HasForeignKey(f => f.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_favorite_professional");

            builder.HasOne(f => f.Client)
                .WithMany(u => u.FavoriteClients)
                .HasForeignKey(f => f.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_favorite_client");
        }
    }
}
