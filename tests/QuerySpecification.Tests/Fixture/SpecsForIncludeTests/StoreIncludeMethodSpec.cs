namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreIncludeMethodSpec : Specification<Store>
{
    public StoreIncludeMethodSpec()
    {
        Query.Include(x => Store.GetSomethingFromStore());
    }
}
