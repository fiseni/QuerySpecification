using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreByIdIncludeAddressSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByIdIncludeAddressSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Address);
    }
}