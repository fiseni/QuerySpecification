using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreSearchByNameSpec : Specification<Store>
    {
        public StoreSearchByNameSpec(string searchTerm)
        {
            Query.Search(x => x.Name!, "%" + searchTerm + "%");
        }
    }
}
