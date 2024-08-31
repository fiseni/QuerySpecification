using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreWithPostProcessingActionSpec : Specification<Store>
{
    public StoreWithPostProcessingActionSpec()
    {
        Query.PostProcessingAction(x => x);
    }
}