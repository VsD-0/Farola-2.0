using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.RevokeAllOtherSessions
{
    public class RevokeAllOtherSessionsBadRequestExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "Password is required"
            };
        }
    }
}
