namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeProductsSpec : Specification<Store>
{
    public StoreIncludeProductsSpec()
    {
        Query.Include(x => x.Products);
    }
}
