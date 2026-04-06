using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.Revoke
{
    public class RevokeSessionUnauthorizedExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Unauthorized",
                Status = 401,
                Detail = "Invalid password"
            };
        }
    }
}
