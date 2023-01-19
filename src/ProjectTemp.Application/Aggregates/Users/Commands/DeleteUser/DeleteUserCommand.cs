using Mediator;

namespace ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string Username { get; set; }
}