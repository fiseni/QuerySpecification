using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreWithPostProcessingActionSpec : Specification<Store>
    {
        public StoreWithPostProcessingActionSpec()
        {
            Query.PostProcessingAction(x => x);
        }
    }
}