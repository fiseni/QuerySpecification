using Microsoft.EntityFrameworkCore;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using PozitronDev.SpecSampleApp.Infrastructure.DataAccess.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Infrastructure.DataAccess
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new StoreConfiguration());
        }
    }
}
