using Xunit;

namespace dbEnumerator.Test.Infrastructure
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}