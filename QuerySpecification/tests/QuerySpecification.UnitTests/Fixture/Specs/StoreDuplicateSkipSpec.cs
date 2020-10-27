using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreDuplicateSkipSpec : Specification<Store>
    {
        public StoreDuplicateSkipSpec()
        {
            Query.Skip(1)
                 .Skip(2);
        }
    }
}