using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreIncludeNameSpec : Specification<Store>
    {
        public StoreIncludeNameSpec()
        {
            Query.Include(x => x.Name);
        }
    }
}
