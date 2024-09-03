using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

[CollectionDefinition("WriteCollection")]
public class WriteCollection : ICollectionFixture<DatabaseFixture>
{
    public WriteCollection()
    {

    }
}
