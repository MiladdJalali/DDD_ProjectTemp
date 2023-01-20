using ProjectTemp.Domain.Aggregates.Users;

namespace ProjectTemp.Application.Aggregates.Users;

public interface IUserWriteRepository
{
    void Add(User user);

    Task<User?> GetByUsername(string username, CancellationToken cancellationToken = default);

    void Remove(User user);
}