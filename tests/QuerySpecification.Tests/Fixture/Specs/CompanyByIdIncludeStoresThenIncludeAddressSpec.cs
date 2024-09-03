namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdIncludeStoresThenIncludeAddressSpec : Specification<Company>
{
    public CompanyByIdIncludeStoresThenIncludeAddressSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Address);
    }
}
