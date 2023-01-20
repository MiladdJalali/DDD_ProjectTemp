using Microsoft.EntityFrameworkCore;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users;

namespace ProjectTemp.Infrastructure.Aggregates.Users;

public class UserWriteRepository : IUserWriteRepository
{
    private readonly WriteDbContext dbContext;

    public UserWriteRepository(WriteDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(User user)
    {
        dbContext.Users.Add(user);
    }

    public Task<User?> GetByUsername(string username, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.FromSqlRaw(@"
                    SELECT      U.*
                    FROM        ""Users"" AS U
                    WHERE       U.""Username"" = {0}
                ", username)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void Remove(User user)
    {
        user.Delete();
        dbContext.Users.Remove(user);
    }
}