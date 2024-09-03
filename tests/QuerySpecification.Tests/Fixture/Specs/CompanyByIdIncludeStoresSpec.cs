namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdIncludeStoresSpec : Specification<Company>
{
    public CompanyByIdIncludeStoresSpec(int companyId)
    {
        Query.Where(x => x.Id == companyId).Include(x => x.Stores);
    }
}
