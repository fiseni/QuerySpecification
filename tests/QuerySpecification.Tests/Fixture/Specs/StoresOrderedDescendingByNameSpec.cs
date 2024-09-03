namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresOrderedDescendingByNameSpec : Specification<Store>
{
    public StoresOrderedDescendingByNameSpec()
    {
        Query.OrderByDescending(x => x.Name);
    }
}
