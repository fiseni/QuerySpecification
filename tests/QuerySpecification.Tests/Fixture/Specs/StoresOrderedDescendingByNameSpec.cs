using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoresOrderedDescendingByNameSpec : Specification<Store>
    {
        public StoresOrderedDescendingByNameSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}