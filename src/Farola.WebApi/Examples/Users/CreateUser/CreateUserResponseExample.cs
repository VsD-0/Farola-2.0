using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Users.CreateUser
{
    public class CreateUserResponseExample : IExamplesProvider<int>
    {
        public int GetExamples() => 5;
    }
}
