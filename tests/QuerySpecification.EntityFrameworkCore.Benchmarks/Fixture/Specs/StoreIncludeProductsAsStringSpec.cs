namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

public class StoreIncludeProductsAsStringSpec : Specification<Store>
{
    public StoreIncludeProductsAsStringSpec(int id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(nameof(Store.Products))
            .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}");
    }
}
