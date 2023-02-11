using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemp.Infrastructure;
using ProjectTemp.RestApi.IntegrationTest.SeedHelpers;
using Xunit;

namespace ProjectTemp.RestApi.IntegrationTest
{
    public class TestFixture: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly TestServer testServer;

        public TestFixture()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(
                        Encoding.ASCII.GetBytes(
                            JsonSerializer.Serialize(new
                            {
                                ConnectionStrings = new
                                {
                                    DefaultConnection =
                                        $"Server={Environment.GetEnvironmentVariable("POSTGRESQL_HOST")};" +
                                        $"Port={Environment.GetEnvironmentVariable("POSTGRESQL_PORT")};" +
                                        $"Database=ProjectTemp{DateTime.Now.Ticks};" +
                                        $"User Id={Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME")};" +
                                        $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")};"
                                }
                            })
                        )
                    )
                ).Build();

            var builder = new WebHostBuilder()
                .ConfigureServices(services => services.AddAutofac())
                .UseEnvironment("Production")
                .UseConfiguration(configurationRoot);

            testServer = new TestServer(builder);

            SeedHelper.EnsureSeedData(testServer.Services).GetAwaiter().GetResult();
            HttpClient = testServer.CreateClient();
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            var dbContext = testServer.Services.GetRequiredService<WriteDbContext>();

            dbContext.Database.EnsureDeleted();
            HttpClient.Dispose();
            testServer.Dispose();
        }
    }
}