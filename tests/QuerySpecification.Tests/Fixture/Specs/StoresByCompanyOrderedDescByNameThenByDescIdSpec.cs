namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresByCompanyOrderedDescByNameThenByDescIdSpec : Specification<Store>
{
    public StoresByCompanyOrderedDescByNameThenByDescIdSpec(int companyId)
    {
        Query.Where(x => x.CompanyId == companyId)
             .OrderByDescending(x => x.Name)
             .ThenByDescending(x => x.Id);
    }
}
