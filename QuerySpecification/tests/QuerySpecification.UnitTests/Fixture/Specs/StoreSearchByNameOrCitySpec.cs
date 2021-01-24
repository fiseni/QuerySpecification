using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreSearchByNameOrCitySpec : Specification<Store>
    {
        public StoreSearchByNameOrCitySpec(string searchTerm)
        {
            Query.Search(x => x.Name!, "%" + searchTerm + "%")
                .Search(x => x.City!, "%" + searchTerm + "%");
        }
    }
}
