using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreByIdIncludeProductsSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByIdIncludeProductsSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Products);
    }
}