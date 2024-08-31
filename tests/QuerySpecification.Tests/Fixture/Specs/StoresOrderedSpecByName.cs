using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoresOrderedSpecByName : Specification<Store>
    {
        public StoresOrderedSpecByName()
        {
            Query.OrderBy(x => x.Name);
        }
    }
}