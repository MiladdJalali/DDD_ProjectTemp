using MediatR;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserByUsername;

public sealed class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserQueryResult?>
{
    private readonly IUserReadRepository userReadRepository;

    public GetUserByUsernameQueryHandler(IUserReadRepository userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    public Task<UserQueryResult?> Handle(
        GetUserByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        return userReadRepository.GetByUsername(request.Username!, cancellationToken);      
    }
}