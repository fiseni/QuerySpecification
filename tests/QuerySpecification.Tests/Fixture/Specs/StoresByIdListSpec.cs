using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoresByIdListSpec : Specification<Store>
    {
        public StoresByIdListSpec(IEnumerable<int> Ids)
        {
            Query.Where(x => Ids.Contains(x.Id));
        }
    }
}
