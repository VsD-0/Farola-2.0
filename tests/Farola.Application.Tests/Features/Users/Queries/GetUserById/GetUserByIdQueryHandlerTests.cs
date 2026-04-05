using Farola.Application.Features.Users.Queries.GetUserById;
using Farola.Domain.Entities;
using Farola.Domain.Interfaces.Repositories;
using Moq;

namespace Farola.Application.Tests.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _handler = new GetUserByIdQueryHandler(_userRepo.Object);
        }

        [Fact]
        public async Task Handle_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 5;
            var query = new GetUserByIdQuery(userId);
            var user = new User { Id = userId, Name = "Test", Email = "test@example.com" };
            _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
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
