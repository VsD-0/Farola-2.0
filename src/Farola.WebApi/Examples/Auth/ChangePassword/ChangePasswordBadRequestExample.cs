using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.ChangePassword
{
    public class ChangePasswordBadRequestExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Validation Error",
                Status = 400,
                Detail = "One or more validation errors occurred.",
                Extensions = new Dictionary<string, object>
                {
                    ["errors"] = new Dictionary<string, string[]>
                    {
                        ["NewPassword"] = new[] { "New password must be at least 6 characters" }
                    }
                }
            };
        }
    }
}
