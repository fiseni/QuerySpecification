namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeAddressSpec : Specification<Store>
{
    public StoreIncludeAddressSpec()
    {
        Query.Include(x => x.Address);
    }
}
