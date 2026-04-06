using Farola.Application.Features.Sessions.Commands.RevokeSession;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.Revoke
{
    public class RevokeSessionRequestExample : IExamplesProvider<RevokeSessionCommand>
    {
        public RevokeSessionCommand GetExamples()
        {
            return new RevokeSessionCommand(
                DeviceId: "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                Password: "password123"
            );
        }
    }
}
