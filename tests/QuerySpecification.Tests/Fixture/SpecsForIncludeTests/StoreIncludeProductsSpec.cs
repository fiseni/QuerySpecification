using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeProductsSpec : Specification<Store>
{
    public StoreIncludeProductsSpec()
    {
        Query.Include(x => x.Products);
    }
}
