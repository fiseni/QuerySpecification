using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Specs
{
    public class StoreWithAddressAndProductsSpec : Specification<Store>
    {
        public StoreWithAddressAndProductsSpec(int id)
        {
            Query.Where(x => x.Id == id);
            Query.Include(x => x.Address);
            Query.Include(x => x.Products);
        }
    }
}
