using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByCompanyOrderedSpec : Specification<Store>
    {
        public StoresByCompanyOrderedSpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name);
        }
    }
}