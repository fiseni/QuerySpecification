using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoresByIdListSpec : Specification<Store>
{
    public StoresByIdListSpec(IEnumerable<int> Ids)
    {
        Query.Where(x => Ids.Contains(x.Id));
    }
}
