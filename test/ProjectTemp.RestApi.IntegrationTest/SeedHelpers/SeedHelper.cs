using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemp.Infrastructure;

namespace ProjectTemp.RestApi.IntegrationTest.SeedHelpers;

public static class SeedHelper
{
    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<WriteDbContext>();

        UserSeedHelper.Seed(dbContext);

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}