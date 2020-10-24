using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresByCompanyPaginatedOrderedDescByNameSpec : Specification<Store>
    {
        public StoresByCompanyPaginatedOrderedDescByNameSpec(int companyId, int skip, int take)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .Paginate(skip, take)
                 .OrderByDescending(x => x.Name);
        }
    }
}