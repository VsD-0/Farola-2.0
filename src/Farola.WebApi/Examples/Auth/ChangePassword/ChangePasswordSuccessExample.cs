using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.ChangePassword
{
    public class ChangePasswordSuccessExample : IExamplesProvider<object>
    {
        public object GetExamples()
        {
            return new { message = "Password changed successfully. All devices have been logged out." };
        }
    }
}
