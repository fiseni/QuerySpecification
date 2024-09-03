namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeCompanyThenCountrySpec : Specification<Store>
{
    public StoreIncludeCompanyThenCountrySpec()
    {
        Query.Include(x => x.Company)
             .ThenInclude(x => x.Country);
    }
}
