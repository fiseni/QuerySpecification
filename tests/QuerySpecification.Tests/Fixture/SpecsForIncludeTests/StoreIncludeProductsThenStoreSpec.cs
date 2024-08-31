using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
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
