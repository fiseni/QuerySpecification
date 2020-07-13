using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByCompanyOrderedThenBySpec : Specification<Store>
    {
        public StoresByCompanyOrderedThenBySpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name)
                 .ThenBy(x => x.Id);
        }
    }
}