using PozitronDev.QuerySpecification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Specs
{
    public class StoreIncludeProductsSpec : Specification<Store>
    {
        public StoreIncludeProductsSpec()
        {
            Query.Include(x => x.Products);
        }
    }
}
