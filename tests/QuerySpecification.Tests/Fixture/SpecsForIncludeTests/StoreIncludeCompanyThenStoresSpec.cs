using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeCompanyThenStoresSpec : Specification<Store>
{
    public StoreIncludeCompanyThenStoresSpec()
    {
        Query.Include(x => x.Company)
             .ThenInclude(x => x!.Stores);
    }
}
