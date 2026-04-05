using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class RoleTests
    {
        [Fact]
        public void Role_CanBeCreated()
        {
            var role = new Role
            {
                Id = 1,
                Name = "Client"
            };

            Assert.Equal(1, role.Id);
            Assert.Equal("Client", role.Name);
            Assert.NotNull(role.Users);
            Assert.Empty(role.Users);
        }
    }
}
