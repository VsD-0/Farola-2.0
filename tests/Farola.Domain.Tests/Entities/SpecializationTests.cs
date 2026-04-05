using Farola.Domain.Entities;

namespace Farola.Domain.Tests.Entities
{
    public class SpecializationTests
    {
        [Fact]
        public void Specialization_CanBeCreated()
        {
            var spec = new Specialization
            {
                Id = 1,
                Name = "Programmer",
                Photo = "photo.jpg"
            };

            Assert.Equal(1, spec.Id);
            Assert.Equal("Programmer", spec.Name);
            Assert.Equal("photo.jpg", spec.Photo);
            Assert.NotNull(spec.Users);
            Assert.Empty(spec.Users);
        }

        [Fact]
        public void Specialization_Photo_CanBeNull()
        {
            var spec = new Specialization { Name = "Designer" };
            Assert.Null(spec.Photo);
        }
    }
}
