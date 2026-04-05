using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class FavoriteTests
    {
        [Fact]
        public void Favorite_CanBeCreated()
        {
            var favorite = new Favorite
            {
                Id = 1,
                ProfessionalId = 10,
                ClientId = 20
            };

            Assert.Equal(1, favorite.Id);
            Assert.Equal(10, favorite.ProfessionalId);
            Assert.Equal(20, favorite.ClientId);
            Assert.Null(favorite.Professional);
            Assert.Null(favorite.Client);
        }
    }
}
