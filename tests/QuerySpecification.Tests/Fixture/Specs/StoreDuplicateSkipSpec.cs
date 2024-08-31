using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreDuplicateSkipSpec : Specification<Store>
{
    public StoreDuplicateSkipSpec()
    {
        Query.Skip(1)
             .Skip(2);
    }
}