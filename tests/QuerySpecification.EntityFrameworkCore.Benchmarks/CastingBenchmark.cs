using BenchmarkDotNet.Attributes;
using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using Pozitron.QuerySpecification.Tests.Fixture.Entities.Seeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks
{
    [MemoryDiagnoser]
    public class CastingBenchmark
    {
        private readonly int max = 100;
        private readonly List<Store> StoresList;
        private readonly IEnumerable<Store> StoresEnumerable;

        public CastingBenchmark()
        {
            StoresList = StoreSeed.Get();
            StoresEnumerable = StoresList.AsEnumerable();
        }

        [Benchmark]
        public void ListToList()
        {
            for (int i = 0; i < max; i++)
            {
                _ = StoresList.ToList();
            }
        }
        [Benchmark]
        public void ListToListWithCastCheck()
        {
            for (int i = 0; i < max; i++)
            {
                _ = StoresList is List<Store> stores ? stores : StoresList.ToList();
            }
        }
        [Benchmark]
        public void IEnumerableToList()
        {
            for (int i = 0; i < max; i++)
            {
                _ = StoresEnumerable.ToList();
            }
        }
        [Benchmark]
        public void IEnumerableToListWithCastCheck()
        {
            for (int i = 0; i < max; i++)
            {
                _ = StoresEnumerable is List<Store> stores ? stores : StoresList.ToList();
            }
        }
    }
}
