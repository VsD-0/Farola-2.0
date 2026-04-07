using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void User_CanBeCreated()
        {
            var user = new User
            {
                Id = 5,
                RoleId = 1,
                Surname = "Doe",
                Name = "John",
                PhoneNumber = "+123456789",
                Email = "john@example.com",
                Password = "hashed",
                Area = "Moscow",
                Information = "Experienced",
                SpecializationId = 2,
                Photo = "avatar.jpg",
                DateRegistration = DateTime.UtcNow,
                Profession = "Developer",
                Patronymic = "Ivanovich",
                IsClosed = false
            };

            Assert.Equal(5, user.Id);
            Assert.Equal("John", user.Name);
            Assert.Equal("Doe", user.Surname);
            Assert.Equal("john@example.com", user.Email);
            Assert.False(user.IsClosed);
            Assert.NotNull(user.FavoriteClients);
            Assert.Empty(user.FavoriteClients);
            Assert.NotNull(user.FavoriteProfessionals);
            Assert.Empty(user.FavoriteProfessionals);
            Assert.NotNull(user.RefreshTokens);
            Assert.Empty(user.RefreshTokens);
            Assert.NotNull(user.StatementsAsClient);
            Assert.Empty(user.StatementsAsClient);
            Assert.NotNull(user.StatementsAsProfessional);
            Assert.Empty(user.StatementsAsProfessional);
        }

        [Fact]
        public void User_OptionalProperties_CanBeNull()
        {
            var user = new User
            {
                RoleId = 1,
                Surname = "Test",
                Name = "User",
                PhoneNumber = "+1234567890",
                Email = "test@test.com",
                Password = "hash",
                DateRegistration = DateTime.UtcNow,
                IsClosed = false
            };
            Assert.Null(user.Area);
            Assert.Null(user.Information);
            Assert.Null(user.SpecializationId);
            Assert.Null(user.Photo);
            Assert.Null(user.Profession);
            Assert.Null(user.Patronymic);
        }
    }
}
