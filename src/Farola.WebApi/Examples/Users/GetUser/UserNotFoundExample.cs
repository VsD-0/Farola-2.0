using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Users.GetUser
{
    public class UserNotFoundExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Not Found",
                Status = 404,
                Detail = "User with id 999 not found"
            };
        }
    }
}
