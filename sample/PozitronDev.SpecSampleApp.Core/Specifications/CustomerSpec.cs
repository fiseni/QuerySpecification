using PozitronDev.QuerySpecification;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using PozitronDev.SpecSampleApp.Core.Specifications.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Core.Specifications
{
    public class CustomerSpec : Specification<Customer>
    {
        public CustomerSpec(CustomerFilter filter)
        {
            Query.OrderBy(x => x.Name)
                    .ThenByDescending(x => x.Address);

            if (filter.LoadChildren)
                Query.Include(x => x.Stores);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.Name))
                Query.Where(x => x.Name == filter.Name);

            if (!string.IsNullOrEmpty(filter.Email))
                Query.Where(x => x.Email == filter.Email);

            if (!string.IsNullOrEmpty(filter.Address))
                Query.Search(x => x.Address, filter.Address);
        }
    }
}
