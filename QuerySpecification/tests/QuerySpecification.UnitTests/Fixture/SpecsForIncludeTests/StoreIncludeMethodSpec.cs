using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreIncludeMethodSpec : Specification<Store>
    {
        public StoreIncludeMethodSpec()
        {
            Query.Include(x => x.GetSomethingFromStore());
        }
    }
}