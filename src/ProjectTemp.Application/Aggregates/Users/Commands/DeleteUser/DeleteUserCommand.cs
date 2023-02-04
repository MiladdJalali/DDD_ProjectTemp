using MediatR;

namespace ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string Username { get; set; }
}