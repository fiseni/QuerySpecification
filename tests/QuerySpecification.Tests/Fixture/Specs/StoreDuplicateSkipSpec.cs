namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreDuplicateSkipSpec : Specification<Store>
{
    public StoreDuplicateSkipSpec()
    {
        Query.Skip(1)
             .Skip(2);
    }
}
