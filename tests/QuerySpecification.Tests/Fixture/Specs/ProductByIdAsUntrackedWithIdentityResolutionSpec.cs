namespace Pozitron.QuerySpecification.Tests.Fixture;

public class ProductByIdAsUntrackedWithIdentityResolutionSpec : Specification<Product>
{
    public ProductByIdAsUntrackedWithIdentityResolutionSpec(int id)
    {
        Query.Where(product => product.Id == id).AsNoTrackingWithIdentityResolution();
    }
}
