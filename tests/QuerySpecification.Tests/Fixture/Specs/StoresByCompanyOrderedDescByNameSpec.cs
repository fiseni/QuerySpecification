using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoresByCompanyOrderedDescByNameSpec : Specification<Store>
{
    public StoresByCompanyOrderedDescByNameSpec(int companyId)
    {
        Query.Where(x => x.CompanyId == companyId)
             .OrderByDescending(x => x.Name);
    }
}