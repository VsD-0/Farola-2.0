using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews", tb => tb.HasComment("Таблица отзывов клиентов"));

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("id").HasComment("Идентификатор отзыва");
            builder.Property(r => r.StatementId).HasColumnName("statement_id").HasComment("Номер заявления");
            builder.Property(r => r.Grade).HasColumnName("grade").HasComment("Оценка работы");
            builder.Property(r => r.Text).HasColumnName("text").HasComment("Текст отзыва");
            builder.Property(r => r.DateAdded).HasColumnName("date_added").HasDefaultValueSql("now()").HasComment("Дата добавления");

            builder.HasOne(r => r.Statement)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.StatementId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_review_statement");
        }
    }
}
