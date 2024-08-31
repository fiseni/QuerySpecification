using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeProductsThenStoreSpec : Specification<Store>
    {
        public StoreIncludeProductsThenStoreSpec()
        {
            Query.Include(x => x.Products)
                 .ThenInclude(x=>x!.Store);
        }
    }
}
