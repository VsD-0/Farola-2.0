using Farola.Application.Features.Sessions.Queries.GetSessions;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Validators
{
    public class GetSessionsQueryValidatorTests
    {
        private readonly GetSessionsQueryValidator _validator = new();

        [Fact]
        public void Validate_ValidQuery_NoErrors()
        {
            var query = new GetSessionsQuery("11111111-1111-1111-1111-111111111111");
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyDeviceId_ShouldHaveError()
        {
            var query = new GetSessionsQuery("");
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.CurrentDeviceId);
        }
    }
}
