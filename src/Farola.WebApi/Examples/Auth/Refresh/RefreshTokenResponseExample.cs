using Farola.Application.Common.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.Refresh
{
    public class RefreshTokenResponseExample : IExamplesProvider<AccessTokenResult>
    {
        public AccessTokenResult GetExamples()
        {
            return new AccessTokenResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
        }
    }
}
