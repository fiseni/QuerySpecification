﻿using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
    {
        public StoreIncludeMethodOfNavigationSpec()
        {
            Query.Include(x => x.Address!.GetSomethingFromAddress());
        }
    }
}