using Farola.Application.Features.Users.Commands.CreateUser;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Farola.WebApi.Tests.Controllers
{
    public class UsersControllerTests : IClassFixture<FarolaWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public UsersControllerTests(FarolaWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateUser_WithValidData_ReturnsCreated()
        {
            var request = new CreateUserCommand(
                Email: "newuser@example.com",
                Password: "Password123!",
                Surname: "Doe",
                Name: "John",
                PhoneNumber: "+1234567890",
                RoleId: 1,
                Patronymic: null,
                Profession: null,
                Area: null,
                Information: null,
                SpecializationId: null,
                Photo: null
            );
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Users", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            // Можно проверить Location header
            Assert.NotNull(response.Headers.Location);
        }
    }
}
