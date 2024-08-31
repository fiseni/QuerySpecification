using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreDuplicateTakeSpec : Specification<Store>
    {
        public StoreDuplicateTakeSpec()
        {
            Query.Take(1)
                 .Take(2);
        }
    }
}