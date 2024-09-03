namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoresByCompanyOrderedDescByNameThenByIdSpec : Specification<Store>
{
    public StoresByCompanyOrderedDescByNameThenByIdSpec(int companyId)
    {
        Query.Where(x => x.CompanyId == companyId)
             .OrderByDescending(x => x.Name)
             .ThenBy(x => x.Id);
    }
}
