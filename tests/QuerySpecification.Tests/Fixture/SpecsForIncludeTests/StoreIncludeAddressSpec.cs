using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeAddressSpec : Specification<Store>
    {
        public StoreIncludeAddressSpec()
        {
            Query.Include(x => x.Address);
        }
    }
}