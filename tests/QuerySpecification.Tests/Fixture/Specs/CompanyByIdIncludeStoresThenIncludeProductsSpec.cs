using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class CompanyByIdIncludeStoresThenIncludeProductsSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdIncludeStoresThenIncludeProductsSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Products);
        }
    }
}
