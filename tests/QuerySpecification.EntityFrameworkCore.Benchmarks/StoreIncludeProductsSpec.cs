using PozitronDev.QuerySpecification;
using PozitronDev.QuerySpecification.EntityFrameworkCore;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;

namespace QuerySpecification.Benchmark
{
    public class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec()
        {
            Query.Include(x => x.Products)
                .Include(x => x.Company).ThenInclude(x=>x.Country);
        }
    }
}
