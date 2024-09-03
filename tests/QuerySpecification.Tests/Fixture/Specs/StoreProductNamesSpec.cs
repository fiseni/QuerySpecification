namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreProductNamesSpec : Specification<Store, string?>
{
    public StoreProductNamesSpec()
    {
        Query.SelectMany(s => s.Products.Select(p => p.Name));
    }
}
