using BenchmarkDotNet.Running;
using Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

//BenchmarkRunner.Run<QueryStringBenchmark>();
//BenchmarkRunner.Run<DbQueryBenchmark>();
//BenchmarkRunner.Run<ExpressionBenchmark>();
//return;

var benchmark = new QueryStringBenchmark();

var x1 = benchmark.EFIncludeExpression();
var x11 = benchmark.EFIncludeExpression();
var x2 = benchmark.EFIncludeString();
var x3 = benchmark.SpecIncludeExpression();
var x4 = benchmark.SpecIncludeString();

Console.WriteLine(x1);
Console.WriteLine();
Console.WriteLine(x2);
Console.WriteLine();
Console.WriteLine(x3);
Console.WriteLine();
Console.WriteLine(x4);
