using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreNamesWithPostProcessingActionSpec : Specification<Store, string?>
    {
        public StoreNamesWithPostProcessingActionSpec()
        {
            Query.Select(x=>x.Name)
                 .PostProcessingAction(x => x);
        }
    }
}