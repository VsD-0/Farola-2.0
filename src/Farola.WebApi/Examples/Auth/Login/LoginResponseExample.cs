using Farola.Application.Common.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Login
{
    public class LoginResponseExample : IExamplesProvider<AccessTokenResult>
    {
        public AccessTokenResult GetExamples()
        {
            return new AccessTokenResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");
        }
    }
}
