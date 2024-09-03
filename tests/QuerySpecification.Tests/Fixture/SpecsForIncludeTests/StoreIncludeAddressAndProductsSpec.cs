namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeAddressAndProductsSpec : Specification<Store>
{
    public StoreIncludeAddressAndProductsSpec()
    {
        Query.Include(x => x.Products)
             .Include(x => x!.Address);
    }
}
