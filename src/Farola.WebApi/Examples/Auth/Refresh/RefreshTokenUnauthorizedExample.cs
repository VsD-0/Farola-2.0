using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Refresh
{
    public class RefreshTokenUnauthorizedExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Unauthorized",
                Status = 401,
                Detail = "Invalid or expired refresh token"
            };
        }
    }
}
