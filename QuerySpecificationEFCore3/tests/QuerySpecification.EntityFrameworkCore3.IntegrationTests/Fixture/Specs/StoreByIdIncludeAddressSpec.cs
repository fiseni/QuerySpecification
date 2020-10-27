using PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Specs
{
    public class StoreByIdIncludeAddressSpec : Specification<Store>
    {
        public StoreByIdIncludeAddressSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Address);
        }
    }
}