using Farola.Application.Features.Auth.Commands.Login;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Login
{
    public class LoginRequestExample : IExamplesProvider<LoginCommand>
    {
        public LoginCommand GetExamples()
        {
            return new LoginCommand(
                Email: "client@example.com",
                Password: "password123",
                DeviceId: "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                DeviceName: "Chrome на Windows 11"
            );
        }
    }
}
