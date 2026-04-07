using AutoMapper;
using Farola.Application.DTOs.Users;
using Farola.Application.Features.Users.Queries.GetUserById;
using Farola.Domain.Constants;
using Farola.Domain.Entities;
using Farola.Domain.Enums;
using Farola.Domain.Interfaces.Repositories;
using Moq;

namespace Farola.Application.Tests.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _handler = new GetUserByIdQueryHandler(_userRepo.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 5;
            var query = new GetUserByIdQuery(userId);
            var role = new Role { Id = (int)RoleType.Client, Name = RoleNames.Client };
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns((User src) => new UserDto
                {
                    Id = src.Id,
                    Name = src.Name,
                    Email = src.Email,
                    Surname = src.Surname,
                    PhoneNumber = src.PhoneNumber,
                    RoleName = src.Role?.Name ?? string.Empty,
                    RoleId = src.RoleId,
                    DateRegistration = src.DateRegistration,
                    IsClosed = src.IsClosed,
                    Area = src.Area,
                    Information = src.Information,
                    SpecializationId = src.SpecializationId,
                    Photo = src.Photo,
                    Profession = src.Profession,
                    Patronymic = src.Patronymic
                });

            var user = new User
            {
                Id = userId,
                Name = "Test",
                Email = "test@example.com",
                Surname = "TestSurname",
                PhoneNumber = "+123456789",
                Role = role,
                RoleId = role.Id,
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Client", result.RoleName);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsNull()
        {
            // Arrange
            var query = new GetUserByIdQuery(999);
            _userRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
