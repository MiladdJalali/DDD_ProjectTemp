using ProjectTemp.Application.Aggregates.Users.Commands.CreateUser;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.CreateUser;

public static class CreateUserCommandBuilder
{
    public static CreateUserCommand Build()
    {
        return new CreateUserCommand
        {
            Username = "UserUsername",
            Password = "UserPassword",
            Description = "UserDescription"
        };
    }
}