using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Data.Seeds
{
    public class StoreSeed
    {
        public static List<Store> Get()
        {
            var stores = new List<Store>();

            for (int i = 1; i <= 50; i++)
            {
                stores.Add(new Store()
                {
                    Id = i,
                    Name = $"Store {i}",
                    AddressId = i,
                    CompanyId = 1,
                });
            }
            for (int i = 51; i <= 100; i++)
            {
                stores.Add(new Store()
                {
                    Id = i,
                    Name = $"Store {i}",
                    AddressId = i,
                    CompanyId = 2,
                });
            }

            return stores;
        }
    }
}
