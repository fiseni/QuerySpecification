using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresOrderedDescendingSpec : Specification<Store>
    {
        public StoresOrderedDescendingSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}