using AutoMapper;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using PozitronDev.SpecSampleApp.Core.Interfaces;
using PozitronDev.SpecSampleApp.Core.Specifications;
using PozitronDev.SpecSampleApp.Core.Specifications.Filters;
using PozitronDev.SpecSampleApp.Web.Interfaces;
using PozitronDev.SpecSampleApp.Web.Models;
using PozitronDev.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PozitronDev.SpecSampleApp.Web.Services
{
    public class CustomerUiService : ICustomerUiService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Customer> customerRepository;

        public CustomerUiService(IMapper mapper,
                                 IRepository<Customer> customerRepository)
        {
            this.mapper = mapper;
            this.customerRepository = customerRepository;
        }


        // Here I'm just writing various usages, not necessarilly you'll need all of them.

        public async Task<CustomerDto> GetCustomer(int customerId)
        {
            var customer = await customerRepository.GetByIdAsync(customerId);

            customer.ValidateFor().NotFound(customerId);

            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetCustomer(string customerName)
        {
            var customer = await customerRepository.GetBySpecAsync(new CustomerByNameSpec(customerName));

            customer.ValidateFor().NotFound(customerName);

            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetCustomerWithStores(string customerName)
        {
            var customer = await customerRepository.GetBySpecAsync(new CustomerByNameWithStoresSpec(customerName));

            customer.ValidateFor().NotFound(customerName);

            return mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetCustomers(CustomerFilterDto filterDto)
        {
            var spec = new CustomerSpec(mapper.Map<CustomerFilter>(filterDto));
            var customer = await customerRepository.ListAsync(spec);

            return mapper.Map<List<CustomerDto>>(customer);
        }
    }
}
