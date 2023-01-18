using FluentAssertions;
using ProjectTemp.Domain.ValueObjects;
using Xunit;

namespace ProjectTemp.Domain.UnitTest.ValueObjects
{
    public class DescriptionUnitTest
    {
        [Theory]
        [InlineData("Description")]
        [InlineData(" Description ")]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues(string value)
        {
            var description = Description.Create(value);

            description?.Value.Should().Be(value.Trim());
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestCreate_WhenValueIsNullOrWhiteSpace_MustReturnNull(string value)
        {
            var description = Description.Create(value);

            description.Should().BeNull();
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const string value = "Description";
            var first = Description.Create(value);
            var second = Description.Create(value);

            first.Should().Be(second);
        }
    }
}