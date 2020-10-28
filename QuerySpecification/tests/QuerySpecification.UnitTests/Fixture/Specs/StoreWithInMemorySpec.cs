using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreWithInMemorySpec : Specification<Store>
    {
        public StoreWithInMemorySpec()
        {
            Query.InMemory(x => x);
        }
    }
}