using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreNamesPaginatedSpec : Specification<Store, string?>
{
    public StoreNamesPaginatedSpec(int skip, int take)
    {
        Query.OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take);

        Query.Select(x => x.Name);
    }
}