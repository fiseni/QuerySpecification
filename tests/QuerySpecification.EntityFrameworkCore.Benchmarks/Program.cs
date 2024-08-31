using BenchmarkDotNet.Running;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CastingBenchmark>();
    }
}
