using PozitronDev.QuerySpecification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Specs
{
    public class StoreIncludeNameSpec : Specification<Store>
    {
        public StoreIncludeNameSpec()
        {
            Query.Include(x => x.Name);
        }
    }
}
