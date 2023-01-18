using System;
using FluentAssertions;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.Exceptions;
using ProjectTemp.Domain.Properties;
using Xunit;

namespace ProjectTemp.Domain.UnitTest.Aggregates.Users.ValueObjects
{
    public class UserUsernameUnitTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void TestCreate_WhenValueIsEmpty_ThrowsException(string value)
        {
            var action = new Action(() => UserUsername.Create(value));

            action.Should().Throw<DomainException>().WithMessage(DomainResources.User_UsernameCannotBeEmpty);
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string value = "UserUsername";
            var username = UserUsername.Create(value);

            username.Value.Should().Be(value);
        }

        [Fact]
        public void TestEquality_WhenEverythingIsOk_MustBeTrue()
        {
            const string value = "UserUsername";
            var first = UserUsername.Create(value);
            var second = UserUsername.Create(value);

            first.Should().Be(second);
        }
    }
}