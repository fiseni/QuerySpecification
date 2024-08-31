using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreWithFaultyIncludeSpec : Specification<Store>
    {
        public StoreWithFaultyIncludeSpec()
        {
            Query.Include(x => x.Id == 1 && x.Name == "Something");
        }
    }
}