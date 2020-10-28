using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreNamesWithInMemorySpec : Specification<Store, string?>
    {
        public StoreNamesWithInMemorySpec()
        {
            Query.Select(x=>x.Name)
                 .InMemory(x => x);
        }
    }
}