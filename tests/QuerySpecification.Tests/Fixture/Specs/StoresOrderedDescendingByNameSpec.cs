using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoresOrderedDescendingByNameSpec : Specification<Store>
{
    public StoresOrderedDescendingByNameSpec()
    {
        Query.OrderByDescending(x => x.Name);
    }
}