using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Farola.Domain.Entities;

namespace Farola.Infrastructure.Data.Configurations
{
    public class StatementConfiguration : IEntityTypeConfiguration<Statement>
    {
        public void Configure(EntityTypeBuilder<Statement> builder)
        {
            builder.ToTable("statements", tb => tb.HasComment("Таблица заявлений"));

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id").HasComment("Идентификатор заявления");
            builder.Property(s => s.ProfessionalId).HasColumnName("professional_id").HasComment("Номер специалиста");
            builder.Property(s => s.ClientId).HasColumnName("client_id").HasComment("Номер клиента");
            builder.Property(s => s.StatusId).HasColumnName("status_id").HasComment("Номер статуса заявления");
            builder.Property(s => s.DateAdded).HasColumnName("date_added").HasDefaultValueSql("now()").HasComment("Дата создания");
            builder.Property(s => s.DateExpiration).HasColumnName("date_expiration").HasComment("Дата закрытия заявки");
            builder.Property(s => s.Grade).HasColumnName("grade").HasComment("Оценка специалиста на заказ");
            builder.Property(s => s.Comment).HasColumnName("comment").HasComment("Комментарий специалиста");

            // Индексы для ускорения поиска
            builder.HasIndex(s => s.ProfessionalId).HasDatabaseName("ix_statements_professional_id");
            builder.HasIndex(s => s.ClientId).HasDatabaseName("ix_statements_client_id");
            builder.HasIndex(s => s.StatusId).HasDatabaseName("ix_statements_status_id");

            builder.HasOne(s => s.Professional)
                .WithMany(u => u.StatementsAsProfessional)
                .HasForeignKey(s => s.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_statement_professional");

            builder.HasOne(s => s.Client)
                .WithMany(u => u.StatementsAsClient)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_statement_client");

            builder.HasOne(s => s.Status)
                .WithMany(ss => ss.Statements)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_statement_status");
        }
    }
}
