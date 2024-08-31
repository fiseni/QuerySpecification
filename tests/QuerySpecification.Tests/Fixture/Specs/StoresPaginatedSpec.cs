using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoresPaginatedSpec : Specification<Store>
    {
        public StoresPaginatedSpec(int skip, int take)
        {
            Query.OrderBy(s => s.Id);
            Query.Paginate(skip, take);
        }
    }
}