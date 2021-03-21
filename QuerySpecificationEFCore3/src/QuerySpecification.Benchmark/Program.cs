using BenchmarkDotNet.Running;

namespace QuerySpecification.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<CastingBenchmark>();
        }
    }
}
