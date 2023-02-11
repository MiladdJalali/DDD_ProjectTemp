using System;
using ProjectTemp.Domain.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Infrastructure;

namespace ProjectTemp.RestApi.IntegrationTest.SeedHelpers;

public static class UserSeedHelper
{
    public const string DefaultUsername = "TestUser";

    public const string DefaultPassword = "TestPassword";

    public static Guid DefaultId { get; } = Guid.NewGuid();

    public static void Seed(WriteDbContext dbContext)
    {
        var defaultUser = User.Create(
            UserId.Create(DefaultId),
            UserUsername.Create(DefaultUsername),
            UserPassword.Create(DefaultPassword)
        );

        dbContext.Users.Add(defaultUser);
    }
}