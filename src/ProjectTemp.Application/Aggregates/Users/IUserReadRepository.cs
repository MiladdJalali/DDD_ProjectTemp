using ProjectTemp.Application.Aggregates.Users.Queries;

namespace ProjectTemp.Application.Aggregates.Users;

public interface IUserReadRepository
{
    IQueryable<UserQueryResult> GetAll();

    ValueTask<UserQueryResult?> GetByUsername(string username, CancellationToken cancellationToken = default);
}