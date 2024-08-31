﻿using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
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
