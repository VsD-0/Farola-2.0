using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class ReviewTests
    {
        [Fact]
        public void Review_CanBeCreated()
        {
            var review = new Review
            {
                Id = 1,
                StatementId = 100,
                Grade = 4.5f,
                Text = "Good work!",
                DateAdded = DateTime.UtcNow
            };

            Assert.Equal(1, review.Id);
            Assert.Equal(100, review.StatementId);
            Assert.Equal(4.5f, review.Grade);
            Assert.Equal("Good work!", review.Text);
            Assert.NotEqual(default, review.DateAdded);
            Assert.Null(review.Statement);
        }
    }
}
