namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdIncludeAddressSpec : Specification<Store>
{
    public StoreByIdIncludeAddressSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Address);
    }
}
