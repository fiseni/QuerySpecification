using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoresOrderedSpecByName : Specification<Store>
{
    public StoresOrderedSpecByName()
    {
        Query.OrderBy(x => x.Name);
    }
}