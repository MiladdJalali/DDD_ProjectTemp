using MediatR;

namespace ProjectTemp.Application.Aggregates.Users.Queries.GetUserCollection;

public class GetUserCollectionQuery : BaseCollectionQuery, IRequest<BaseCollectionResult<UserQueryResult>>
{
}