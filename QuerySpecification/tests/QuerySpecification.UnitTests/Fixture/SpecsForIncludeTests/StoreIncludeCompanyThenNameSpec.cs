using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyThenNameSpec : Specification<Store>
    {
        public StoreIncludeCompanyThenNameSpec()
        {
            Query.Include(x => x.Company)
                 .ThenInclude(x=>x!.Name);
        }
    }
}
