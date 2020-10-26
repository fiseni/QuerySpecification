using PozitronDev.QuerySpecification;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Core.Specifications
{
    public class CustomerByNameWithStoresSpec : Specification<Customer>
    {
        public CustomerByNameWithStoresSpec(string name)
        {
            Query.Where(x => x.Name == name)
                .Include(x => x.Stores);
        }
    }
}
