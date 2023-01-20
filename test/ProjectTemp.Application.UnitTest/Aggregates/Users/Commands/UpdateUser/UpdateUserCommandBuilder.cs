using ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.UpdateUser;

public static class UpdateUserCommandBuilder
{
    public static UpdateUserCommand Build()
    {
        return new UpdateUserCommand
        {
            CurrentUsername = "UserUsername",
            Username = "UpdatedUserUsername",
            Description = "UpdatedUserDescription"
        };
    }
}