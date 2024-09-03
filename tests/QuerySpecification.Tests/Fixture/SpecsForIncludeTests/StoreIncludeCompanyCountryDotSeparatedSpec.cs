namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
{
    public StoreIncludeCompanyCountryDotSeparatedSpec()
    {
        Query.Include(x => x.Company.Country);
    }
}
