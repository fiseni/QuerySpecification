using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pozitron.QuerySpecification;
using Pozitron.QuerySpecification.EntityFrameworkCore;
using Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using Pozitron.QuerySpecification.Tests.Fixture.Entities.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks
{
    [MemoryDiagnoser]
    public class SpecIncludeBenchmarkAsync
    {
        private readonly int max = 1000000;
        private readonly SpecificationEvaluator evaluator = SpecificationEvaluator.Default;
        private readonly Specification<Store> specInclude = new StoreIncludeProductsSpec();
        private readonly Specification<Store> specIncludeString = new StoreIncludeProductsAsStringSpec();

        private readonly IQueryable<Store> Stores;

        public SpecIncludeBenchmarkAsync()
        {
            Stores = new BenchmarkDbContext().Stores.AsQueryable();
        }

        //[Benchmark]
        public async Task EFIncludeExpressionAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < max; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    _ = Stores.Include(x => x.Products)
                            .Include(x => x.Company).ThenInclude(x => x.Country);
                }));
            }

            await Task.WhenAll(tasks);
        }

        //[Benchmark]
        public async Task EFIncludeStringAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < max; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    _ = Stores.Include(nameof(Store.Products))
                                .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}");
                }));
            }

            await Task.WhenAll(tasks);
        }

        //[Benchmark]
        public async Task SpecIncludeExpressionAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < max; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    _ = evaluator.GetQuery(Stores, specInclude);
                }));
            }

            await Task.WhenAll(tasks);
        }

        //[Benchmark]
        public async Task SpecIncludeStringAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < max; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    _ = evaluator.GetQuery(Stores, specIncludeString);
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}
