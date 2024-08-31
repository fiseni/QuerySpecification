using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeNameSpec : Specification<Store>
{
    public StoreIncludeNameSpec()
    {
        Query.Include(x => x.Name);
    }
}
