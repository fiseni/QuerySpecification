namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresOrderedTwoChainsSpec : Specification<Store>
{
    public StoresOrderedTwoChainsSpec()
    {
        Query.OrderBy(x => x.Name)
            .OrderBy(x => x.Id);
    }
}
