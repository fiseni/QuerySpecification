using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyThenCountrySpec : Specification<Store>
    {
        public StoreIncludeCompanyThenCountrySpec()
        {
            Query.Include(x => x.Company)
                 .ThenInclude(x=>x!.Country);
        }
    }
}
