using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoresPaginatedSpec : Specification<Store>
    {
        public StoresPaginatedSpec(int skip, int take)
        {
            Query.Skip(skip)
                 .Take(take);
        }
    }
}