﻿using Microsoft.EntityFrameworkCore;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities.Seeds;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture
{
    public class IntegrationTestBase
    {
        public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEFTestsDB;";
        protected TestDbContext dbContext;
        protected Repository<Company> companyRepository;
        protected Repository<Store> storeRepository;

        public IntegrationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            dbContext = new TestDbContext(optionsBuilder.Options);

            dbContext.Database.EnsureCreated();

            companyRepository = new Repository<Company>(dbContext);
            storeRepository = new Repository<Store>(dbContext);
        }
    }
}