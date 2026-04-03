using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Services;
using Farola.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Farola.Infrastructure.Data
{
    public class DbSeeder
    {
        private readonly FarolaDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public DbSeeder(FarolaDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync(FarolaDbContext context)
        {
            if (await context.Roles.AnyAsync())
                return;

            // 1. Роли
            var roles = new List<Role>
        {
            new Role { Id = 1, Name = "Client" },
            new Role { Id = 2, Name = "Professional" },
            new Role { Id = 3, Name = "Admin" }
        };
            await _context.Roles.AddRangeAsync(roles);

            // 2. Статусы заявлений
            var statuses = new List<StatementStatus>
        {
            new StatementStatus { Id = 1, Name = "Created" },
            new StatementStatus { Id = 2, Name = "InProgress" },
            new StatementStatus { Id = 3, Name = "Completed" },
            new StatementStatus { Id = 4, Name = "Cancelled" }
        };
            await _context.StatementStatuses.AddRangeAsync(statuses);

            // 3. Специализации
            var specializations = new List<Specialization>
        {
            new Specialization { Name = "Программист", Photo = "programmer.jpg" },
            new Specialization { Name = "Дизайнер", Photo = "designer.jpg" },
            new Specialization { Name = "Маркетолог", Photo = "marketer.jpg" }
        };
            await _context.Specializations.AddRangeAsync(specializations);
            await _context.SaveChangesAsync();

            // 4. Пользователи
            var client = new User
            {
                RoleId = roles.First(r => r.Name == "Client").Id,
                Surname = "Петров",
                Name = "Петр",
                Patronymic = "Петрович",
                PhoneNumber = "+79991112233",
                Email = "client@example.com",
                Password = _passwordHasher.HashPassword("hashed_password_here"),
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
                Password = _passwordHasher.HashPassword("hashed_password_here"),
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
                Password = _passwordHasher.HashPassword("hashed_password_here"),
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };

            await _context.Users.AddRangeAsync(client, professional, admin);
            await _context.SaveChangesAsync();
        }
    }
}
