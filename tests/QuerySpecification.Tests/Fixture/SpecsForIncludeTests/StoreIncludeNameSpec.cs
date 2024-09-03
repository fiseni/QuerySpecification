namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeNameSpec : Specification<Store>
{
    public StoreIncludeNameSpec()
    {
        Query.Include(x => x.Name);
    }
}
