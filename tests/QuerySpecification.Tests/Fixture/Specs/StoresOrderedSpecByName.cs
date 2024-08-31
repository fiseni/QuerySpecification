using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoresOrderedSpecByName : Specification<Store>
    {
        public StoresOrderedSpecByName()
        {
            Query.OrderBy(x => x.Name);
        }
    }
}