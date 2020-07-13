using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresOrderedSpecByName : Specification<Store>
    {
        public StoresOrderedSpecByName()
        {
            Query.OrderBy(x => x.Name);
        }
    }
}