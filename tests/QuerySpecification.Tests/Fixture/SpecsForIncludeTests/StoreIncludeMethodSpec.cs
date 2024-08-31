using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeMethodSpec : Specification<Store>
{
    public StoreIncludeMethodSpec()
    {
        Query.Include(x => x.GetSomethingFromStore());
    }
}