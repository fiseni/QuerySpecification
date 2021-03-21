using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
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