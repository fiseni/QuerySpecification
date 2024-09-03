namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdIncludeProductsSpec : Specification<Store>
{
    public StoreByIdIncludeProductsSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Products);
    }
}
