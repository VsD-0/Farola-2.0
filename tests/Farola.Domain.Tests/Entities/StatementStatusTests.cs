using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class StatementStatusTests
    {
        [Fact]
        public void StatementStatus_CanBeCreated()
        {
            var status = new StatementStatus
            {
                Id = 1,
                Name = "Created"
            };

            Assert.Equal(1, status.Id);
            Assert.Equal("Created", status.Name);
            Assert.NotNull(status.Statements);
            Assert.Empty(status.Statements);
        }
    }
}
