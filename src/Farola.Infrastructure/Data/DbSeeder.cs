using Farola.Domain.Entities;
using Farola.Infrastructure.Data;
using Farola.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Farola.Infrastructure.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(FarolaDbContext context)
        {
            // Проверяем, есть ли уже данные, чтобы не перезаписывать
            if (await context.Roles.AnyAsync())
                return;

            // 1. Роли
            var roles = new List<Role>
        {
            new Role { Id = 1, Name = "Client" },
            new Role { Id = 2, Name = "Professional" },
            new Role { Id = 3, Name = "Admin" }
        };
            await context.Roles.AddRangeAsync(roles);

            // 2. Статусы заявлений
            var statuses = new List<StatementStatus>
        {
            new StatementStatus { Id = 1, Name = "Created" },
            new StatementStatus { Id = 2, Name = "InProgress" },
            new StatementStatus { Id = 3, Name = "Completed" },
            new StatementStatus { Id = 4, Name = "Cancelled" }
        };
            await context.StatementStatuses.AddRangeAsync(statuses);

            // 3. Специализации
            var specializations = new List<Specialization>
        {
            new Specialization { Name = "Программист", Photo = "programmer.jpg" },
            new Specialization { Name = "Дизайнер", Photo = "designer.jpg" },
            new Specialization { Name = "Маркетолог", Photo = "marketer.jpg" }
        };
            await context.Specializations.AddRangeAsync(specializations);
            await context.SaveChangesAsync(); // сохраняем, чтобы получить Id

            // 4. Пользователи
            var client = new User
            {
                RoleId = roles.First(r => r.Name == "Client").Id,
                Surname = "Петров",
                Name = "Петр",
                Patronymic = "Петрович",
                PhoneNumber = "+79991112233",
                Email = "client@example.com",
                Password = "hashed_password_here", // временно, в реальном проекте хэшируйте!
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            var professional = new User
            {
                RoleId = roles.First(r => r.Name == "Professional").Id,
                Surname = "Иванов",
                Name = "Иван",
                Patronymic = "Иванович",
                PhoneNumber = "+79994445566",
                Email = "professional@example.com",
                Password = "hashed_password_here",
                Profession = "Разработчик C#",
                SpecializationId = specializations.First(s => s.Name == "Программист").Id,
                Area = "Москва",
                Information = "Опыт 5 лет, специализируюсь на бэкенде.",
                Photo = "ivanov.jpg",
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            var admin = new User
            {
                RoleId = roles.First(r => r.Name == "Admin").Id,
                Surname = "Администратор",
                Name = "Админ",
                PhoneNumber = "+79990000000",
                Email = "admin@example.com",
                Password = "hashed_password_here",
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            await context.Users.AddRangeAsync(client, professional, admin);
            await context.SaveChangesAsync();
        }
    }
}
