using PozitronDev.QuerySpecification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Specs
{
    public class StoreIncludeAddressSpec : Specification<Store>
    {
        public StoreIncludeAddressSpec()
        {
            Query.Include(x => x.Address);
        }
    }
}