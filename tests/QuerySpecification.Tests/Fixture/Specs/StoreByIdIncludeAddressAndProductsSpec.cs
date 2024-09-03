namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdIncludeAddressAndProductsSpec : Specification<Store>
{
    public StoreByIdIncludeAddressAndProductsSpec(int id)
    {
        Query.Where(x => x.Id == id);
        Query.Include(x => x.Address);
        Query.Include(x => x.Products);
    }
}
