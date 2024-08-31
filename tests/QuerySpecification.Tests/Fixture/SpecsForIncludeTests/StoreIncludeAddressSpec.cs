using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeAddressSpec : Specification<Store>
{
    public StoreIncludeAddressSpec()
    {
        Query.Include(x => x.Address);
    }
}