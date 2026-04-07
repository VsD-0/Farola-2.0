using Farola.Application.Features.Auth.Commands.ChangePassword;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Auth.ChangePassword
{
    public class ChangePasswordRequestExample : IExamplesProvider<ChangePasswordCommand>
    {
        public ChangePasswordCommand GetExamples()
        {
            return new ChangePasswordCommand("oldPassword123", "newPassword456");
        }
    }
}
