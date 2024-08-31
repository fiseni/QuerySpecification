using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoresByCompanyPaginatedOrderedDescByNameSpec : Specification<Store>
{
    public StoresByCompanyPaginatedOrderedDescByNameSpec(int companyId, int skip, int take)
    {
        Query.Where(x => x.CompanyId == companyId)
             .Skip(skip)
             .Take(take)
             .OrderByDescending(x => x.Name);
    }
}