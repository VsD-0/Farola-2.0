using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.Revoke
{
    public class RevokeSessionNotFoundExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Not Found",
                Status = 404,
                Detail = "Session with specified DeviceId not found"
            };
        }
    }
}
