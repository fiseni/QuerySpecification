using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
{
    public StoreIncludeCompanyCountryDotSeparatedSpec()
    {
        Query.Include(x => x.Company!.Country);
    }
}