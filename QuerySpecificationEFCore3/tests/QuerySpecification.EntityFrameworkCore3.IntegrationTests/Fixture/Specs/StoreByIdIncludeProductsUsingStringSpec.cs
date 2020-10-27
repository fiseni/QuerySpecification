using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Specs
{
    public class StoreByIdIncludeProductsUsingStringSpec : Specification<Store>
    {
        public StoreByIdIncludeProductsUsingStringSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(nameof(Store.Products));
        }
    }
}