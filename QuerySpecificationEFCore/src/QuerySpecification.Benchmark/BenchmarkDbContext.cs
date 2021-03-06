﻿using Microsoft.EntityFrameworkCore;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySpecification.Benchmark
{
    public class BenchmarkDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }

        public BenchmarkDbContext()
        {
        }
        public BenchmarkDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QuerySpecificationBenchmark;ConnectRetryCount=0");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Store>().HasOne(x => x.Address).WithOne(x => x.Store).HasForeignKey<Address>(x => x.StoreId);
        }
    }
}
