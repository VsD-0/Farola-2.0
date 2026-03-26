using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens", tb => tb.HasComment("Токены обновления"));

            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Id).HasColumnName("id").HasComment("Идентификатор токена");
            builder.Property(rt => rt.UserId).HasColumnName("userid").HasComment("Идентификатор пользователя");
            builder.Property(rt => rt.Token).HasColumnName("token").HasMaxLength(255).HasComment("Токен");
            builder.Property(rt => rt.CreatedAt).HasColumnName("createdat").HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Дата и время создания");
            builder.Property(rt => rt.ExpiresAt).HasColumnName("expiresat").HasComment("Дата и время истечения срока действия");

            // Индекс для быстрого поиска по токену
            builder.HasIndex(rt => rt.Token).IsUnique().HasDatabaseName("ix_refresh_tokens_token");

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_token_user");
        }
    }
}
