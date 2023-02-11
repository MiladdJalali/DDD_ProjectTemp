using Xunit;

namespace ProjectTemp.RestApi.IntegrationTest
{
    [CollectionDefinition(nameof(TestFixtureCollection))]
    public class TestFixtureCollection : ICollectionFixture<TestFixture>
    {
    }
}