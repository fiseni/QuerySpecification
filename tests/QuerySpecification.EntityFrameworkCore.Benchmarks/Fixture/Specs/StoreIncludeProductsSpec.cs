namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

public class StoreIncludeProductsSpec : Specification<Store>
{
    public StoreIncludeProductsSpec(int id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Products)
            .Include(x => x.Company).ThenInclude(x => x.Country);
    }
}
