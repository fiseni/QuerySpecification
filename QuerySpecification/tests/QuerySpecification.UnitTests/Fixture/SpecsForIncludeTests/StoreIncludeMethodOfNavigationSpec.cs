using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
    {
        public StoreIncludeMethodOfNavigationSpec()
        {
            Query.Include(x => x.Address!.GetSomethingFromAddress());
        }
    }
}