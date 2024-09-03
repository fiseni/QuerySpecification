namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
{
    public StoreIncludeMethodOfNavigationSpec()
    {
        Query.Include(x => x.Address.GetSomethingFromAddress());
    }
}
