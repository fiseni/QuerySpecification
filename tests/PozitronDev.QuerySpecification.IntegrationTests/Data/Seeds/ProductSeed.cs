﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Data.Seeds
{
    public class ProductSeed
    {
        public static List<Product> Get()
        {
            var products = new List<Product>();

            for (int i = 1; i < 100; i=i+2)
            {
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product {i}",
                    StoreId = i,
                });
                products.Add(new Product()
                {
                    Id = i + 1,
                    Name = $"Product {i + 1}",
                    StoreId = i,
                });
            }

            return products;
        }
    }
}
