using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByIdListSpec : Specification<Store>
    {
        public StoresByIdListSpec(IEnumerable<int> Ids)
        {
            Query.Where(x => Ids.Contains(x.Id));
        }
    }
}
