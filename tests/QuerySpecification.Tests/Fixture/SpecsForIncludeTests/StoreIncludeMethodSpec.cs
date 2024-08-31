using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreIncludeMethodSpec : Specification<Store>
    {
        public StoreIncludeMethodSpec()
        {
            Query.Include(x => x.GetSomethingFromStore());
        }
    }
}