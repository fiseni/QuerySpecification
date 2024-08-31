using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
{
    public StoreIncludeCompanyThenCountryAsStringSpec()
    {
        Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
    }
}
