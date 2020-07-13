using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresPaginatedSpec : Specification<Store>
    {
        public StoresPaginatedSpec(int take, int skip)
        {
            Query.Paginate(take, skip);
        }
    }
}