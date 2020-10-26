using PozitronDev.QuerySpecification;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Core.Specifications
{
    public class CustomerByNameSpec : Specification<Customer>
    {
        public CustomerByNameSpec(string name)
        {
            Query.Where(x => x.Name == name)
                 .OrderBy(x => x.Name)
                    .ThenByDescending(x => x.Address);
        }
    }
}
