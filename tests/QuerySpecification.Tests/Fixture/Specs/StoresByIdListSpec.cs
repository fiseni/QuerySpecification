namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresByIdListSpec : Specification<Store>
{
    public StoresByIdListSpec(IEnumerable<int> ids)
    {
        Query.Where(x => ids.Contains(x.Id));
    }
}
