﻿using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
    {
        public StoreIncludeCompanyCountryDotSeparatedSpec()
        {
            Query.Include(x => x.Company!.Country);
        }
    }
}