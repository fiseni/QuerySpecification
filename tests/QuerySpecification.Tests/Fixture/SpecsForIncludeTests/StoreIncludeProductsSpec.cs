using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec()
        {
            Query.Include(x => x.Products);
        }
    }
}
