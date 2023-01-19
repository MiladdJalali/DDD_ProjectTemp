using Mediator;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserByFullName;

public class GetUserByFullNameQuery : IRequest<UserQueryResult>
{
    public string Username { get; set; }
}