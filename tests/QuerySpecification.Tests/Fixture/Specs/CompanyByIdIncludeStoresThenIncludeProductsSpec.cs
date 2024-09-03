namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdIncludeStoresThenIncludeProductsSpec : Specification<Company>
{
    public CompanyByIdIncludeStoresThenIncludeProductsSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Products);
    }
}
