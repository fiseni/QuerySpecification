namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdIncludeProductsUsingStringSpec : Specification<Store>
{
    public StoreByIdIncludeProductsUsingStringSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(nameof(Store.Products));
    }
}
