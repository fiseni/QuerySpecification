using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoreNamesPaginatedSpec : Specification<Store, string>
    {
        public StoreNamesPaginatedSpec(int take, int skip)
        {
            Query.Paginate(take, skip);
            Query.Select(x => x.Name);
        }
    }
}