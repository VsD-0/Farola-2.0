namespace Farola.Application.DTOs.Users
{
    /// <summary>
    /// DTO для передачи данных пользователя.
    /// </summary>
    public class UserDto
    {
        /// <summary>Идентификатор пользователя.</summary>
        public int Id { get; set; }

        /// <summary>Фамилия.</summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>Имя.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Отчество (опционально).</summary>
        public string? Patronymic { get; set; }

        /// <summary>Номер телефона.</summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>Email.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Регион/город.</summary>
        public string? Area { get; set; }

        /// <summary>Дополнительная информация.</summary>
        public string? Information { get; set; }

        /// <summary>Идентификатор специализации (для профессионалов).</summary>
        public int? SpecializationId { get; set; }

        /// <summary>Фото профиля.</summary>
        public string? Photo { get; set; }

        /// <summary>Дата регистрации.</summary>
        public DateTime DateRegistration { get; set; }

        /// <summary>Профессия (для профессионалов).</summary>
        public string? Profession { get; set; }

        /// <summary>Статус профиля (открыт/закрыт).</summary>
        public bool IsClosed { get; set; }

        /// <summary>Идентификатор роли.</summary>
        public int RoleId { get; set; }

        /// <summary>Название роли.</summary>
        public string RoleName { get; set; } = string.Empty;
    }
}
