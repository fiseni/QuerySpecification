using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
    {
        public StoreIncludeCompanyThenCountryAsStringSpec()
        {
            Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
        }
    }
}
