namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdAsSplitQuery : Specification<Company>
{
    public CompanyByIdAsSplitQuery(int id)
    {
        Query.Where(company => company.Id == id)
            .Include(x => x.Stores)
            .ThenInclude(x => x.Products)
            .AsSplitQuery();
    }
}
