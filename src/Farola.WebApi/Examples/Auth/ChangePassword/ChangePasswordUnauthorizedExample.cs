using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.ChangePassword
{
    public class ChangePasswordUnauthorizedExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Unauthorized",
                Status = 401,
                Detail = "Invalid old password"
            };
        }
    }
}
