using PozitronDev.QuerySpecification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Specs
{
    public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
    {
        public StoreIncludeMethodOfNavigationSpec()
        {
            Query.Include(x => x.Address!.GetSomethingFromAddress());
        }
    }
}