using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
{
    public StoreIncludeMethodOfNavigationSpec()
    {
        Query.Include(x => x.Address!.GetSomethingFromAddress());
    }
}