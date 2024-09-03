namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreNamesSpec : Specification<Store, string?>
{
    public StoreNamesSpec()
    {
        Query.Select(x => x.Name);
    }
}
