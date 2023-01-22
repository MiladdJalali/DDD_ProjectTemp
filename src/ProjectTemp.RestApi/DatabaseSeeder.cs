using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemp.Application;
using ProjectTemp.Domain.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Infrastructure;

namespace ProjectTemp.RestApi;

public static class DatabaseSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        var dbContext = serviceProvider.GetService<WriteDbContext>();

        try
        {
            await unitOfWork.BeginTransaction().ConfigureAwait(false);
            await SeedUsers(dbContext, configuration).ConfigureAwait(false);
            await unitOfWork.CommitTransaction().ConfigureAwait(false);
        }
        catch
        {
            await unitOfWork.RollbackTransaction().ConfigureAwait(false);
        }
    }

    private static async Task SeedUsers(WriteDbContext dbContext, IConfiguration configuration)
    {
        if (await dbContext.Users.AnyAsync(i => i.Id.Value == ApplicationConstants.AdminUserId)
                .ConfigureAwait(false))
            return;

        dbContext.Users.Add(
            User.Create(
                UserId.Create(ApplicationConstants.AdminUserId),
                UserUsername.Create(ApplicationConstants.AdminUsername),
                UserPassword.Create(configuration["AdminPassword"]!)
            )
        );
    }
}