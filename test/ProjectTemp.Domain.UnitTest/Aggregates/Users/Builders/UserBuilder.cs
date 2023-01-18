using System;
using ProjectTemp.Domain.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;

namespace ProjectTemp.Domain.UnitTest.Aggregates.Users.Builders;

public class UserBuilder
{
    private Guid id;

    private string password;

    private string username;

    public UserBuilder()
    {
        id = Guid.NewGuid();
        username = "Username";
        password = "Password";
    }

    public User Build()
    {
        return User.Create(
            UserId.Create(id),
            UserUsername.Create(username),
            UserPassword.Create(password));
    }

    public UserBuilder WithId(Guid value)
    {
        id = value;
        return this;
    }

    public UserBuilder WithUsername(string value)
    {
        username = value;
        return this;
    }

    public UserBuilder WithPassword(string value)
    {
        password = value;
        return this;
    }
}