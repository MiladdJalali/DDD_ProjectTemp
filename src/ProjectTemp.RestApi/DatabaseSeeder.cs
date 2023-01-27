using System;
using System.Linq;
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
            SeedUsers(dbContext, configuration);
            await unitOfWork.CommitTransaction().ConfigureAwait(false);
        }
        catch
        {
            await unitOfWork.RollbackTransaction().ConfigureAwait(false);
        }
    }

    private static void SeedUsers(WriteDbContext dbContext, IConfiguration configuration)
    {
        if (dbContext.Users.Any() && dbContext.Users.Any(i => i.Id.Value == ApplicationConstants.AdminUserId))
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