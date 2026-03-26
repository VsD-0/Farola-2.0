using Farola.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Farola.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", tb => tb.HasComment("Таблица пользователей системы"));

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").HasComment("Идентификатор пользователя");

            builder.Property(u => u.RoleId).HasColumnName("role_id").HasComment("Номер роли");
            builder.Property(u => u.Surname).HasColumnName("surname").HasMaxLength(100).HasComment("Фамилия пользователя");
            builder.Property(u => u.Name).HasColumnName("name").HasMaxLength(50).HasComment("Имя пользователя");
            builder.Property(u => u.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).HasComment("Номер телефона");
            builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(100).HasComment("Электронная почта");
            builder.Property(u => u.Password).HasColumnName("password").HasMaxLength(255).HasComment("Пароль");
            builder.Property(u => u.Area).HasColumnName("area").HasMaxLength(50).HasComment("Место работы");
            builder.Property(u => u.Information).HasColumnName("information").HasComment("Подробная информация");
            builder.Property(u => u.SpecializationId).HasColumnName("specialization_id").HasComment("Номер специализации");
            builder.Property(u => u.Photo).HasColumnName("photo").HasMaxLength(80).HasComment("Имя фото");
            builder.Property(u => u.DateRegistration).HasColumnName("date_registration").HasDefaultValueSql("now()").HasComment("Дата регистрации");
            builder.Property(u => u.Profession).HasColumnName("profession").HasMaxLength(100).HasComment("Профессия");
            builder.Property(u => u.Patronymic).HasColumnName("patronymic").HasMaxLength(80).HasComment("Отчество");
            builder.Property(u => u.IsClosed).HasColumnName("is_closed").HasDefaultValue(false).HasComment("Статус профиля специалиста (открыт/закрыт)");

            // Индексы
            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("ix_users_email");
            builder.HasIndex(u => u.PhoneNumber).IsUnique().HasDatabaseName("ix_users_phone");

            // Связи
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict) // нельзя удалить роль, если есть пользователи
                .HasConstraintName("fk_user_role");

            builder.HasOne(u => u.Specialization)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SpecializationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user_specialization");
        }
    }
}
