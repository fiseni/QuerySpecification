using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeProductsThenStoreSpec : Specification<Store>
{
    public StoreIncludeProductsThenStoreSpec()
    {
        Query.Include(x => x.Products)
             .ThenInclude(x => x!.Store);
    }
}
