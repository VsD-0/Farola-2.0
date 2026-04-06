using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Refresh
{
    public class RefreshTokenBadRequestExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "Refresh token not found in cookie"
            };
        }
    }
}
