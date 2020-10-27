﻿using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoresByCompanyPaginatedSpec : Specification<Store>
    {
        public StoresByCompanyPaginatedSpec(int companyId, int skip, int take)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .Skip(skip)
                 .Take(take);
        }
    }
}