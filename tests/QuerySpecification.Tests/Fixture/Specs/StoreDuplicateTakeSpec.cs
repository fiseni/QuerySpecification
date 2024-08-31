using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
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