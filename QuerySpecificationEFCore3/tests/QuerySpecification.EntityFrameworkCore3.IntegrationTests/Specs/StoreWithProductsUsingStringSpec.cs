using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Specs
{
    public class StoreWithProductsUsingStringSpec : Specification<Store>
    {
        public StoreWithProductsUsingStringSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(nameof(Store.Products));
        }
    }
}