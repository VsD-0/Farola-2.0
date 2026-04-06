using Farola.Application.Features.Users.Commands.CreateUser;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Users.CreateUser
{
    public class CreateUserRequestExample : IExamplesProvider<CreateUserCommand>
    {
        public CreateUserCommand GetExamples()
        {
            return new CreateUserCommand(
                Email: "client@example.com",
                Password: "password123",
                Surname: "Петров",
                Name: "Пётр",
                PhoneNumber: "+79991234567",
                RoleId: 1,
                Patronymic: "Иванович",
                Profession: "Разработчик",
                Area: "Москва",
                Information: "Опыт 5 лет",
                SpecializationId: 2,
                Photo: "avatar.jpg"
            );
        }
    }
}
