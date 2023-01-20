using ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.DeleteUser;

public static class DeleteUserCommandBuilder
{
    public static DeleteUserCommand Build()
    {
        return new DeleteUserCommand
        {
            Username = "UserName"
        };
    }
}