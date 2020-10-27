using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Specs
{
    public class CompanyByIdIncludeStoresThenIncludeProductsSpec : Specification<Company>
    {
        public CompanyByIdIncludeStoresThenIncludeProductsSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Products);
        }
    }
}
