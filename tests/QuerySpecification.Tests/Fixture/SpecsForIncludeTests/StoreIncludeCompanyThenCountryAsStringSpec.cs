namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
{
    public StoreIncludeCompanyThenCountryAsStringSpec()
    {
        Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
    }
}
