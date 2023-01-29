﻿using Mediator;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserCollection;

public sealed class GetUserCollectionQueryHandler :
    IRequestHandler<GetUserCollectionQuery, BaseCollectionResult<UserQueryResult>>
{
    private readonly IUserReadRepository userReadRepository;

    public GetUserCollectionQueryHandler(IUserReadRepository userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    public ValueTask<BaseCollectionResult<UserQueryResult>> Handle(
            GetUserCollectionQuery request, 
            CancellationToken cancellationToken)
    {
        var result = userReadRepository.GetAll().OrderBy(i => i.Username);

        var resultWithPaging = result.ApplyPaging(request.PageSize, request.PageIndex).ToArray();

        return ValueTask.FromResult(new BaseCollectionResult<UserQueryResult>
        {
            Result = resultWithPaging,
            TotalCount = resultWithPaging.Length
        });
    }
}