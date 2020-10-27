using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Specs
{
    public class StoreByIdIncludeAddressAndProductsSpec : Specification<Store>
    {
        public StoreByIdIncludeAddressAndProductsSpec(int id)
        {
            Query.Where(x => x.Id == id);
            Query.Include(x => x.Address);
            Query.Include(x => x.Products);
        }
    }
}
