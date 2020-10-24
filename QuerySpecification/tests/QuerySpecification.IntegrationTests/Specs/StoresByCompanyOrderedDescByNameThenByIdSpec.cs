using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByCompanyOrderedDescByNameThenByIdSpec : Specification<Store>
    {
        public StoresByCompanyOrderedDescByNameThenByIdSpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name)
                 .ThenBy(x => x.Id);
        }
    }
}