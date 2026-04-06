using Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsRequestExample : IExamplesProvider<RevokeAllOtherSessionsCommand>
    {
        public RevokeAllOtherSessionsCommand GetExamples()
        {
            return new RevokeAllOtherSessionsCommand(Password: "password123");
        }
    }
}
