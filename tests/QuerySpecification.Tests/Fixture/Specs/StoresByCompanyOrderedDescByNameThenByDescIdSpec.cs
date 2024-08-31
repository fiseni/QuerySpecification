using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoresByCompanyOrderedDescByNameThenByDescIdSpec : Specification<Store>
    {
        public StoresByCompanyOrderedDescByNameThenByDescIdSpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name)
                 .ThenByDescending(x => x.Id);
        }
    }
}