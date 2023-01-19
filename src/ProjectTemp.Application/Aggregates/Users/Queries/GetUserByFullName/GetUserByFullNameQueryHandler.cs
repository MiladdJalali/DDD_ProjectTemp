using Mediator;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserByFullName;

public class GetUserByFullNameQueryHandler : IRequestHandler<GetUserByFullNameQuery, UserQueryResult>
{
    private readonly IUserReadRepository userReadRepository;

    public GetUserByFullNameQueryHandler(IUserReadRepository userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    ValueTask<UserQueryResult> IRequestHandler<GetUserByFullNameQuery, UserQueryResult>.Handle(
        GetUserByFullNameQuery request,
        CancellationToken cancellationToken)
    {
        return userReadRepository.GetByUsername(request.Username, cancellationToken);
    }
}