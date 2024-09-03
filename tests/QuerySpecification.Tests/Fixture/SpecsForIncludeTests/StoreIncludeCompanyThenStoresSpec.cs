namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeCompanyThenStoresSpec : Specification<Store>
{
    public StoreIncludeCompanyThenStoresSpec()
    {
        Query.Include(x => x.Company)
            .ThenInclude(x => x.Stores)
            .ThenInclude(x => x.Products);
    }
}
