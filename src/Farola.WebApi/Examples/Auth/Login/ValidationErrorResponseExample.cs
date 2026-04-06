using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Login
{
    public class ValidationErrorResponseExample : IExamplesProvider<ProblemDetails>
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
                        ["Email"] = new[] { "Email is required", "Invalid email format" },
                        ["Password"] = new[] { "Password must be at least 4 characters" }
                    }
                }
            };
        }
    }
}
