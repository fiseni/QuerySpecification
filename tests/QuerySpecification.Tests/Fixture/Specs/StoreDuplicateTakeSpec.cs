namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreDuplicateTakeSpec : Specification<Store>
{
    public StoreDuplicateTakeSpec()
    {
        Query.Take(1)
             .Take(2);
    }
}
