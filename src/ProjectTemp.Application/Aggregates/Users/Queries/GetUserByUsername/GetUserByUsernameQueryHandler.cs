﻿using Mediator;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserQueryResult?>
{
    private readonly IUserReadRepository userReadRepository;

    public GetUserByUsernameQueryHandler(IUserReadRepository userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    ValueTask<UserQueryResult?> IRequestHandler<GetUserByUsernameQuery, UserQueryResult>.Handle(
        GetUserByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        return userReadRepository.GetByUsername(request.Username!, cancellationToken);
    }
}