using PozitronDev.QuerySpecification;
using PozitronDev.QuerySpecification.EntityFrameworkCore3;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;

namespace QuerySpecification.Benchmark
{
    public class StoreIncludeProductsAsStringSpec : Specification<Store>
    {
        public StoreIncludeProductsAsStringSpec()
        {
            Query.Include(nameof(Store.Products))
                .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}");
        }
    }
}
