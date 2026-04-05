using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class StatementTests
    {
        [Fact]
        public void Statement_CanBeCreated()
        {
            var statement = new Statement
            {
                Id = 1,
                ProfessionalId = 10,
                ClientId = 20,
                StatusId = 3,
                DateAdded = DateTime.UtcNow,
                DateExpiration = DateTime.UtcNow.AddDays(7),
                Grade = 5.0f,
                Comment = "Great!"
            };

            Assert.Equal(1, statement.Id);
            Assert.Equal(10, statement.ProfessionalId);
            Assert.Equal(20, statement.ClientId);
            Assert.Equal(3, statement.StatusId);
            Assert.NotEqual(default, statement.DateAdded);
            Assert.NotNull(statement.DateExpiration);
            Assert.Equal(5.0f, statement.Grade);
            Assert.Equal("Great!", statement.Comment);
            Assert.Null(statement.Professional);
            Assert.Null(statement.Client);
            Assert.Null(statement.Status);
            Assert.NotNull(statement.Reviews);
            Assert.Empty(statement.Reviews);
        }

        [Fact]
        public void Statement_OptionalProperties_CanBeNull()
        {
            var statement = new Statement
            {
                ProfessionalId = 1,
                ClientId = 2,
                StatusId = 1,
                DateAdded = DateTime.UtcNow
            };
            Assert.Null(statement.DateExpiration);
            Assert.Null(statement.Grade);
            Assert.Null(statement.Comment);
        }
    }
}
