using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreByIdIncludeProductsUsingStringSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByIdIncludeProductsUsingStringSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(nameof(Store.Products));
    }
}