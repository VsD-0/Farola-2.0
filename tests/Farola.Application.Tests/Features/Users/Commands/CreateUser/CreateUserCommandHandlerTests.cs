using Farola.Application.Features.Users.Commands.CreateUser;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;
using Moq;

namespace Farola.Application.Tests.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<IRoleCacheService> _roleCache = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _handler = new CreateUserCommandHandler(
                _userRepo.Object,
                _hasher.Object,
                _roleCache.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesUser()
        {
            var command = new CreateUserCommand(
                Email: "new@example.com",
                Password: "password123",
                Surname: "Doe",
                Name: "John",
                PhoneNumber: "+1234567890",
                RoleId: 1,
                Patronymic: null,
                Profession: null,
                Area: null,
                Information: null,
                SpecializationId: null,
                Photo: null
            );

            var role = new Role { Id = 1, Name = "Client" };
            _roleCache.Setup(r => r.GetRoleByNameAsync("Client", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Role { Id = 1, Name = "Client" });
            _userRepo.Setup(r => r.EmailExistsAsync(command.Email))
                .ReturnsAsync(false);
            _hasher.Setup(h => h.HashPassword(command.Password)).Returns("hashed_password");

            var userId = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(0, userId);
            _userRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Email == command.Email &&
                u.Password == "hashed_password" &&
                u.RoleId == role.Id
            )), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_EmailAlreadyExists_ThrowsInvalidOperationException()
        {
            var command = new CreateUserCommand("existing@example.com", "pass", "Doe", "John", "+123", 1);
            _userRepo.Setup(r => r.EmailExistsAsync(command.Email))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            _userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RoleNotFound_ThrowsInvalidOperationException()
        {
            var command = new CreateUserCommand("new@example.com", "pass", "Doe", "John", "+123", 999);
            _userRepo.Setup(r => r.EmailExistsAsync(command.Email))
                .ReturnsAsync(false);
            _roleCache.Setup(r => r.GetRoleByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            _userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
