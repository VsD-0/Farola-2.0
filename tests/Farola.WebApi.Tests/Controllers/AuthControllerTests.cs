using Farola.Application.Common.Models;
using Farola.Application.Features.Users.Commands.CreateUser;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Farola.WebApi.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<FarolaWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(FarolaWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsAccessTokenAndSetsCookie()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var email = $"logintest_{unique}@example.com";
            var deviceId = Guid.NewGuid().ToString();
            var createCommand = new CreateUserCommand(
                Email: email,
                Password: "Test123!",
                Surname: "Login",
                Name: "Test",
                PhoneNumber: $"+{unique}",
                RoleId: 1,
                Patronymic: null, Profession: null, Area: null, Information: null, SpecializationId: null, Photo: null
            );
            var createResponse = await _client.PostAsJsonAsync("/api/Users", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var loginCommand = new { email, password = "Test123!", deviceId, deviceName = "TestDevice" };
            var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);
            loginResponse.EnsureSuccessStatusCode();

            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            var responseString = await loginResponse.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AccessTokenResult>(responseString);
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);

            if (!loginResponse.Headers.TryGetValues("Set-Cookie", out var cookieValues))
            {
                var body = await loginResponse.Content.ReadAsStringAsync();
                Assert.Fail($"No Set-Cookie header. Status: {loginResponse.StatusCode}, Body: {body}");
            }

            var setCookieHeader = cookieValues.FirstOrDefault();
            Assert.NotNull(setCookieHeader);
            Assert.Contains("refreshToken=", setCookieHeader);
            Assert.Contains("HttpOnly", setCookieHeader);
        }

        [Fact]
        public async Task Refresh_WithValidCookie_ReturnsNewAccessToken()
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var email = $"refresh_{unique}@example.com";
            var deviceId = Guid.NewGuid().ToString();
            var createCommand = new CreateUserCommand(
                Email: email,
                Password: "Test123!",
                Surname: "Refresh",
                Name: "Test",
                PhoneNumber: $"+{unique}",
                RoleId: 1,
                Patronymic: null, Profession: null, Area: null, Information: null, SpecializationId: null, Photo: null
            );
            var createResponse = await _client.PostAsJsonAsync("/api/Users", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var loginCommand = new { email, password = "Test123!", deviceId, deviceName = "RefreshDevice" };
            var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);
            loginResponse.EnsureSuccessStatusCode();

            var refreshResponse = await _client.PostAsync("/api/Auth/refresh", new StringContent("", Encoding.UTF8, "application/json"));
            refreshResponse.EnsureSuccessStatusCode();

            var refreshResult = await refreshResponse.Content.ReadFromJsonAsync<AccessTokenResult>();
            Assert.NotNull(refreshResult);
            Assert.NotEmpty(refreshResult.AccessToken);
        }
    }
}
