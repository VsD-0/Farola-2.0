using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.GetSessions
{
    public class MissingDeviceIdErrorExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "X-Device-Id header is required"
            };
        }
    }
}
