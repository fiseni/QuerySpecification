using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreDuplicateTakeSpec : Specification<Store>
{
    public StoreDuplicateTakeSpec()
    {
        Query.Take(1)
             .Take(2);
    }
}