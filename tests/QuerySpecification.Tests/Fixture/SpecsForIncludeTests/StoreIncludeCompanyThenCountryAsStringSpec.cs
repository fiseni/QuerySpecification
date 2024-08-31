using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
    {
        public StoreIncludeCompanyThenCountryAsStringSpec()
        {
            Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
        }
    }
}
