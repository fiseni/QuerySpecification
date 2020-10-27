using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreSearchByNameSpec : Specification<Store>
    {
        public StoreSearchByNameSpec(string searchTerm)
        {
            Query.Search(x => x.Name!, searchTerm);
        }
    }
}
