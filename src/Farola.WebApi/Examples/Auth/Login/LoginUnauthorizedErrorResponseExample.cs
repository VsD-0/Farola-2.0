using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Login
{
    public class LoginUnauthorizedErrorResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Unauthorized",
                Status = 401,
                Detail = "Invalid credentials"
            };
        }
    }
}
