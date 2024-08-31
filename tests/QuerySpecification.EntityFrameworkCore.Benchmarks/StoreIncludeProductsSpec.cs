using Pozitron.QuerySpecification;
using Pozitron.QuerySpecification.EntityFrameworkCore;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks
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
