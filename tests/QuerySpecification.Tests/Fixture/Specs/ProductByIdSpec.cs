namespace Pozitron.QuerySpecification.Tests.Fixture;

public class ProductByIdSpec : Specification<Product>
{
    public ProductByIdSpec(int id)
    {
        Query.Where(x => x.Id == id);
    }
}
