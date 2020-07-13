using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoresPaginatedSpec : Specification<Store>
    {
        public StoresPaginatedSpec(int skip, int take)
        {
            Query.Paginate(skip, take);
        }
    }
}