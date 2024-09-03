using Xunit;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

[CollectionDefinition("ReadCollection")]
public class ReadCollection : ICollectionFixture<DatabaseFixture>
{
    public ReadCollection()
    {

    }
}
