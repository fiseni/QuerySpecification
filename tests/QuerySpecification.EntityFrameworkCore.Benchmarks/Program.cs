using BenchmarkDotNet.Running;
using Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
return;

var benchmark = new QueryStringBenchmark();

var x1 = benchmark.EFIncludeExpression();
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
