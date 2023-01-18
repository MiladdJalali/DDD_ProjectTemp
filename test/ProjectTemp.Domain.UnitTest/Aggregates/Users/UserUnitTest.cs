using System;
using FluentAssertions;
using ProjectTemp.Domain.Aggregates.Users.Events;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.UnitTest.Aggregates.Users.Builders;
using ProjectTemp.Domain.UnitTest.Helpers;
using ProjectTemp.Domain.ValueObjects;
using Xunit;

namespace ProjectTemp.Domain.UnitTest.Aggregates.Users
{
    public class UserUnitTest
    {
        [Fact]
        public void TestChangeDescription_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string description = "UserDescription";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();

            user.ClearEvents();
            user.ChangeDescription(Description.Create(description));

            var descriptionChangedEvent = user.AssertPublishedDomainEvent<UserDescriptionChangedEvent>();

            descriptionChangedEvent.AggregateId.Should().Be(userId);
            descriptionChangedEvent.OldValue.Should().BeNull();
            descriptionChangedEvent.NewValue.Should().Be(description);
            user.Description?.Value.Should().Be(description);
        }

        [Fact]
        public void TestChangeDescription_WhenValueIsSame_NothingMustBeHappened()
        {
            const string description = "UserDescription";
            var user = new UserBuilder().Build();

            user.ChangeDescription(Description.Create(description));
            user.ClearEvents();

            user.ChangeDescription(Description.Create(description));

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeDescription_WhenEverythingIsOkAndNewValueIsEmpty_ValueMustBeSet()
        {
            const string description = "UserDescription";
            var user = new UserBuilder().Build();

            user.ChangeDescription(Description.Create(description));
            user.ClearEvents();
            user.ChangeDescription(Description.Create(""));

            var descriptionChangedEvent = user.AssertPublishedDomainEvent<UserDescriptionChangedEvent>();

            user.DomainEvents.Should().HaveCount(1);
            descriptionChangedEvent.AggregateId.Should().Be(user.Id.Value);
            descriptionChangedEvent.OldValue.Should().Be(description);
            descriptionChangedEvent.NewValue.Should().BeNull();
        }

        [Fact]
        public void TestChangePassword_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldPassword = "OldUserPassword";
            const string newPassword = "NewUserPassword";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).WithPassword(oldPassword).Build();

            user.ClearEvents();
            user.ChangePassword(UserPassword.Create(newPassword));

            var passwordChangedEvent = user.AssertPublishedDomainEvent<UserPasswordChangedEvent>();

            passwordChangedEvent.AggregateId.Should().Be(userId);
            user.Password.Value.Should().Be(newPassword);
        }

        [Fact]
        public void TestChangePassword_WhenValueIsSame_NothingMustBeHappened()
        {
            const string password = "UserPassword";
            var user = new UserBuilder().WithPassword(password).Build();

            user.ClearEvents();
            user.ChangePassword(UserPassword.Create(password));

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestChangeUsername_WhenEverythingIsOk_ValueMustBeSet()
        {
            const string oldUsername = "OldUserUsername";
            const string newUsername = "NewUserUsername";
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).WithUsername(oldUsername).Build();

            user.ClearEvents();
            user.ChangeUsername(UserUsername.Create(newUsername));

            var usernameChangedEvent = user.AssertPublishedDomainEvent<UserUsernameChangedEvent>();

            usernameChangedEvent.AggregateId.Should().Be(userId);
            usernameChangedEvent.OldValue.Should().Be(oldUsername);
            usernameChangedEvent.NewValue.Should().Be(newUsername);
            user.Username.Value.Should().Be(newUsername);
        }

        [Fact]
        public void TestChangeUsername_WhenValueIsSame_NothingMustBeHappened()
        {
            const string username = "UserUsername";
            var user = new UserBuilder().WithUsername(username).Build();

            user.ClearEvents();
            user.ChangeUsername(UserUsername.Create(username));

            user.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void TestCreate_WhenEverythingIsOk_PropertiesShouldHaveCorrectValues()
        {
            const string username = "UserUsername";
            const string password = "UserPassword";
            var userId = Guid.NewGuid();

            var user = new UserBuilder()
                .WithId(userId)
                .WithUsername(username)
                .WithPassword(password)
                .Build();

            var createdEvent = user.AssertPublishedDomainEvent<UserCreatedEvent>();
            var usernameChangedEvent = user.AssertPublishedDomainEvent<UserUsernameChangedEvent>();
            var passwordChangedEvent = user.AssertPublishedDomainEvent<UserPasswordChangedEvent>();

            createdEvent.AggregateId.Should().Be(userId);
            usernameChangedEvent.AggregateId.Should().Be(userId);
            usernameChangedEvent.OldValue.Should().BeNull();
            usernameChangedEvent.NewValue.Should().Be(username);
            passwordChangedEvent.AggregateId.Should().Be(userId);
            user.Id.Value.Should().Be(userId);
            user.Username.Value.Should().Be(username);
            user.Password.Value.Should().Be(password);
            user.Description.Should().BeNull();
        }

        [Fact]
        public void TestDelete_WhenEverythingIsOk_MustBeMarkedAsDeleted()
        {
            var user = new UserBuilder().Build();

            user.ClearEvents();
            user.Delete();

            var deletedEvent = user.AssertPublishedDomainEvent<UserDeletedEvent>();

            deletedEvent.AggregateId.Should().Be(user.Id.Value);

            user.CanBeDeleted().Should().BeTrue();
            user.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void TestDelete_WhenAlreadyDeleted_ThrowsException()
        {
            var user = new UserBuilder().Build();

            user.Delete();

            var action = new Action(() => user.Delete());
            action.Should().Throw<InvalidOperationException>();
        }
    }
}