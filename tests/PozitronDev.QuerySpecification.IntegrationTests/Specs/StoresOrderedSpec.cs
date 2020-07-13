using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresOrderedSpec : Specification<Store>
    {
        public StoresOrderedSpec()
        {
            Query.OrderBy(x => x.Name);
        }
    }
}