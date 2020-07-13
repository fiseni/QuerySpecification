using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByCompanyPaginatedOrderedSpec : Specification<Store>
    {
        public StoresByCompanyPaginatedOrderedSpec(int companyId, int take, int skip)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .Paginate(take, skip)
                 .OrderByDescending(x => x.Name);
        }
    }
}