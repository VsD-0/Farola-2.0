using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Users.CreateUser
{
    public class CreateUserConflictExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Conflict",
                Status = 409,
                Detail = "User with this email already exists"
            };
        }
    }
}
