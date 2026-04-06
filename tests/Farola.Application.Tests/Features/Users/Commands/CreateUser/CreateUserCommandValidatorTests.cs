using Farola.Application.Features.Users.Commands.CreateUser;
using FluentValidation.TestHelper;

namespace Farola.Application.Tests.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        private readonly CreateUserCommandValidator _validator = new();

        [Fact]
        public void Validate_ValidCommand_NoErrors()
        {
            var command = new CreateUserCommand(
                Email: "test@example.com",
                Password: "password123",
                Surname: "Doe",
                Name: "John",
                PhoneNumber: "+1234567890",
                RoleId: 1,
                Patronymic: "Smith",
                Profession: "Developer",
                Area: "Moscow",
                Information: "Some info",
                SpecializationId: 5,
                Photo: "photo.jpg"
            );
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_EmptyEmail_ShouldHaveError()
        {
            var command = new CreateUserCommand("", "pass123", "Doe", "John", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_InvalidEmailFormat_ShouldHaveError()
        {
            var command = new CreateUserCommand("invalid", "pass123", "Doe", "John", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_EmptyPassword_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "", "Doe", "John", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_PasswordTooShort_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "12345", "Doe", "John", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_EmptySurname_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "pass123", "", "John", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void Validate_EmptyName_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "pass123", "Doe", "", "+123", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_EmptyPhoneNumber_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "pass123", "Doe", "John", "", 1);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Validate_RoleIdZeroOrNegative_ShouldHaveError()
        {
            var command = new CreateUserCommand("test@test.com", "pass123", "Doe", "John", "+123", 0);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.RoleId);
        }
    }
}
