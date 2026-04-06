using Farola.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Users.GetUser
{
    public class GetUserResponseExample : IExamplesProvider<User>
    {
        public User GetExamples()
        {
            return new User
            {
                Id = 1,
                RoleId = 1,
                Surname = "Петров",
                Name = "Пётр",
                PhoneNumber = "+79991234567",
                Email = "client@example.com",
                Password = "hashed_password",
                Area = "Москва",
                Information = "Опыт 5 лет",
                SpecializationId = 2,
                Photo = "avatar.jpg",
                DateRegistration = DateTime.UtcNow,
                Profession = "Разработчик",
                Patronymic = "Иванович",
                IsClosed = false
            };
        }
    }
}
