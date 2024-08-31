using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeCompanyThenNameSpec : Specification<Store>
{
    public StoreIncludeCompanyThenNameSpec()
    {
        Query.Include(x => x.Company)
             .ThenInclude(x => x!.Name);
    }
}
