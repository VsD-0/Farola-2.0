using Farola.Application.Features.Auth.Commands.ChangePassword;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandValidatorTests
    {
        private readonly ChangePasswordCommandValidator _validator = new();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new ChangePasswordCommand("old123", "new456");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyOldPassword_HasError()
        {
            var command = new ChangePasswordCommand("", "new456");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.OldPassword);
        }

        [Fact]
        public void Validate_EmptyNewPassword_HasError()
        {
            var command = new ChangePasswordCommand("old123", "");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validate_NewPasswordTooShort_HasError()
        {
            var command = new ChangePasswordCommand("old123", "12345");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}
