using MediatR;

namespace ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public string? CurrentUsername { get; set; }

    public string? Username { get; set; }

    public string? Description { get; set; }
}