using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreNamesSpec : Specification<Store, string?>
{
    public StoreNamesSpec()
    {
        Query.Select(x => x.Name);
    }
}