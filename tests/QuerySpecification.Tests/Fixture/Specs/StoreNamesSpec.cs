using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreNamesSpec : Specification<Store, string?>
    {
        public StoreNamesSpec()
        {
            Query.Select(x => x.Name);
        }
    }
}