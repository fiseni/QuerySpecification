using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoresOrderedDescendingByNameSpec : Specification<Store>
    {
        public StoresOrderedDescendingByNameSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}