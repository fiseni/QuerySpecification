using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresOrderedDescendingByNameSpec : Specification<Store>
    {
        public StoresOrderedDescendingByNameSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}