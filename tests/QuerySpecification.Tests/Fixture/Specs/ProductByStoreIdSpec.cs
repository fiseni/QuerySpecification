namespace Pozitron.QuerySpecification.Tests.Fixture;

public class ProductByStoreIdSpec : Specification<Product>
{
    public ProductByStoreIdSpec(int storeId)
    {
        Query.Where(x => x.StoreId == storeId);
    }
}
