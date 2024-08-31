using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class StoreWithPostProcessingActionSpec : Specification<Store>
    {
        public StoreWithPostProcessingActionSpec()
        {
            Query.PostProcessingAction(x => x);
        }
    }
}