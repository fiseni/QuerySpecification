using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Data.Seeds
{
    public class AddressSeed
    {
        public static List<Address> Get()
        {
            var addresses = new List<Address>();

            for (int i = 1; i <= 100; i++)
            {
                addresses.Add(new Address()
                {
                    Id = i,
                    Street = $"Street {i}",
                    StoreId = i
                });
            }

            return addresses;
        }
    }
}
