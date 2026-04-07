using Farola.Application.Features.Auth.Commands.Login;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Validators
{
    public class LoginCommandValidatorTests
    {
        private readonly LoginCommandValidator _validator = new();

        [Fact]
        public void Validate_EmptyEmail_ShouldHaveError()
        {
            var command = new LoginCommand("", "pass", "dev", "name");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_InvalidEmailFormat_ShouldHaveError()
        {
            var command = new LoginCommand("invalid", "pass", "dev", "name");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_EmptyPassword_ShouldHaveError()
        {
            var command = new LoginCommand("test@test.com", "", "dev", "name");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_PasswordTooShort_ShouldHaveError()
        {
            var command = new LoginCommand("test@test.com", "123", "dev", "name");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_EmptyDeviceId_ShouldHaveError()
        {
            var command = new LoginCommand("test@test.com", "1234", "", "name");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DeviceId);
        }

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new LoginCommand("test@test.com", "1234", "11111111-1111-1111-1111-111111111111", "My Device");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
