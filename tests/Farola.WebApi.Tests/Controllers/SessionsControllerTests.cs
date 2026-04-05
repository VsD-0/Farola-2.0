using Farola.Application.Common.Models;
using Farola.Application.DTOs.Sessions.Sessions;
using Farola.Application.Features.Users.Commands.CreateUser;
using System.Net;
using System.Net.Http.Json;

namespace Farola.WebApi.Tests.Controllers
{
    public class SessionsControllerTests : IClassFixture<FarolaWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SessionsControllerTests(FarolaWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<(string AccessToken, string DeviceId)> LoginAndGetTokenAsync(string email, string password, string deviceId)
        {
            var loginCommand = new { email, password, deviceId, deviceName = "TestDevice" };
            var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);
            loginResponse.EnsureSuccessStatusCode();
            var result = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResult>();
            return (result!.AccessToken, deviceId);
        }

        private async Task<string> CreateUserAsync(string baseEmail, string password)
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var parts = baseEmail.Split('@');
            var local = parts[0];
            var domain = parts.Length > 1 ? parts[1] : "example.com";
            var finalEmail = $"{local}_{unique}@{domain}";
            var command = new CreateUserCommand(
                Email: finalEmail,
                Password: password,
                Surname: "Test",
                Name: "User",
                PhoneNumber: $"+{unique}",
                RoleId: 1,
                Patronymic: null,
                Profession: null,
                Area: null,
                Information: null,
                SpecializationId: null,
                Photo: null
            );
            var response = await _client.PostAsJsonAsync("/api/Users", command);
            response.EnsureSuccessStatusCode();
            return finalEmail;
        }

        [Fact]
        public async Task GetSessions_WithoutAuthorization_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/Sessions");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetSessions_WithoutDeviceIdHeader_ReturnsBadRequest()
        {
            var password = "Pass123!";
            var deviceId = Guid.NewGuid().ToString();
            var email = await CreateUserAsync("sessions1@test.com", password);
            var (token, _) = await LoginAndGetTokenAsync(email, password, deviceId);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Sessions");
            request.Headers.Add("Authorization", $"Bearer {token}");
            // Заголовок X-Device-Id не добавляем
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetSessions_WithValidRequest_ReturnsOkAndList()
        {
            var password = "Pass123!";
            var deviceId = Guid.NewGuid().ToString();
            var email = await CreateUserAsync("sessions2@test.com", password);
            var (token, _) = await LoginAndGetTokenAsync(email, password, deviceId);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Sessions");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("X-Device-Id", deviceId);
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sessions = await response.Content.ReadFromJsonAsync<List<SessionDto>>();
            Assert.NotNull(sessions);
#pragma warning disable CS8604
            Assert.Single(sessions);
#pragma warning restore CS8604
            Assert.Equal(deviceId, sessions[0].DeviceId);
            Assert.True(sessions[0].IsCurrentDevice);
        }

        [Fact]
        public async Task RevokeSession_WithValidPassword_ReturnsNoContent()
        {
            var password = "Pass123!";
            var deviceId = Guid.NewGuid().ToString();
            var email = await CreateUserAsync("revoke1@test.com", password);
            var (token, _) = await LoginAndGetTokenAsync(email, password, deviceId);

            var deviceId2 = Guid.NewGuid().ToString();
            await LoginAndGetTokenAsync(email, password, deviceId2);

            var revokeCommand = new { deviceId = deviceId2, password };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Sessions/revoke");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("X-Device-Id", deviceId);
            request.Content = JsonContent.Create(revokeCommand);
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task RevokeSession_WithInvalidPassword_ReturnsUnauthorized()
        {
            var password = "Pass123!";
            var deviceId = Guid.NewGuid().ToString();
            var email = await CreateUserAsync("revoke2@test.com", password);
            var (token, _) = await LoginAndGetTokenAsync(email, password, deviceId);

            var revokeCommand = new { deviceId = "any", password = "wrong" };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Sessions/revoke");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("X-Device-Id", deviceId);
            request.Content = JsonContent.Create(revokeCommand);
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RevokeAllOtherSessions_WithValidPassword_RevokesOthers()
        {
            var password = "Pass123!";
            var deviceId = Guid.NewGuid().ToString();
            var email = await CreateUserAsync("revokeall@test.com", password);
            var (token, _) = await LoginAndGetTokenAsync(email, password, deviceId);

            var deviceId2 = Guid.NewGuid().ToString();
            var deviceId3 = Guid.NewGuid().ToString();
            await LoginAndGetTokenAsync(email, password, deviceId2);
            await LoginAndGetTokenAsync(email, password, deviceId3);

            var revokeAllCommand = new { password };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Sessions/revoke-all");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("X-Device-Id", deviceId);
            request.Content = JsonContent.Create(revokeAllCommand);
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Sessions");
            getRequest.Headers.Add("Authorization", $"Bearer {token}");
            getRequest.Headers.Add("X-Device-Id", deviceId);
            var getResponse = await _client.SendAsync(getRequest);
            var sessions = await getResponse.Content.ReadFromJsonAsync<List<SessionDto>>();
#pragma warning disable CS8604
            Assert.Single(sessions);
#pragma warning restore CS8604
            Assert.Equal(deviceId, sessions![0].DeviceId);
        }
    }
}
