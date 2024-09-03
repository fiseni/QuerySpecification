namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresOrderedSpecByName : Specification<Store>
{
    public StoresOrderedSpecByName()
    {
        Query.OrderBy(x => x.Name);
    }
}
