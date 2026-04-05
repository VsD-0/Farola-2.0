using Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Validators
{
    public class RevokeAllOtherSessionsCommandValidatorTests
    {
        private readonly RevokeAllOtherSessionsCommandValidator _validator = new();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new RevokeAllOtherSessionsCommand("password123");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyPassword_ShouldHaveError()
        {
            var command = new RevokeAllOtherSessionsCommand("");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
