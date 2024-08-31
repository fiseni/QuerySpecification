using Pozitron.QuerySpecification;
using Pozitron.QuerySpecification.EntityFrameworkCore;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks
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
