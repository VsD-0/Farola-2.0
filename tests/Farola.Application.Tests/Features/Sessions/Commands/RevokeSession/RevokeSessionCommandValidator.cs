using Farola.Application.Features.Sessions.Commands.RevokeSession;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Features.Sessions.Commands.RevokeSession
{
    public class RevokeSessionCommandValidatorTests
    {
        private readonly RevokeSessionCommandValidator _validator = new();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new RevokeSessionCommand("11111111-1111-1111-1111-111111111111", "password123");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyDeviceId_ShouldHaveError()
        {
            var command = new RevokeSessionCommand("", "password123");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DeviceId);
        }

        [Fact]
        public void Validate_EmptyPassword_ShouldHaveError()
        {
            var command = new RevokeSessionCommand("11111111-1111-1111-1111-111111111111", "");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
