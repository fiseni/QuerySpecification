using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreSearchByNameAndCitySpec : Specification<Store>
    {
        public StoreSearchByNameAndCitySpec(string searchTerm)
        {
            Query.Search(x => x.Name!, "%" + searchTerm + "%", 1)
                .Search(x => x.City!, "%" + searchTerm + "%", 2);
        }
    }
}
