using PozitronDev.QuerySpecification.IntegrationTests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Specs
{
    public class StoreByIdSpec : Specification<Store>
    {
        public StoreByIdSpec(int Id)
        {
            Query.Where(x => x.Id == Id);
        }
    }
}
