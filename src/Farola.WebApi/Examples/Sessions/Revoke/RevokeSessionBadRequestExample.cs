using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.Revoke
{
    public class RevokeSessionBadRequestExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "DeviceId is required"
            };
        }
    }
}
