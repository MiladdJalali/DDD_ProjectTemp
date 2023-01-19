using ProjectTemp.Domain.Aggregates.Users.Events;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.ValueObjects;

namespace ProjectTemp.Domain.Aggregates.Users;

public class User : Entity, IAggregateRoot
{
    private User()
    {
    }

    public UserId Id { get; private set; }

    public UserUsername Username { get; private set; }

    public UserPassword Password { get; private set; }

    public Description? Description { get; private set; }

    public static User Create(
        UserId userId,
        UserUsername username,
        UserPassword password)
    {
        var user = new User {Id = userId};

        user.AddEvent(new UserCreatedEvent(user.Id));
        user.ChangeUsername(username);
        user.ChangePassword(password);

        return user;
    }

    public void ChangeUsername(UserUsername username)
    {
        if (Username == username)
            return;

        AddEvent(new UserUsernameChangedEvent(Id, Username, username));

        Username = username;
    }

    public void ChangePassword(UserPassword password)
    {
        AddEvent(new UserPasswordChangedEvent(Id));

        Password = password;
    }

    public void ChangeDescription(Description? description)
    {
        if (Description?.Value == description?.Value)
            return;

        AddEvent(new UserDescriptionChangedEvent(Id, Description, description));

        Description = description;
    }

    public void Delete()
    {
        if (CanBeDeleted())
            throw new InvalidOperationException();

        AddEvent(new UserDeletedEvent(Id));
        MarkAsDeleted();
    }
}