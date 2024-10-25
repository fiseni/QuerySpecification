using BenchmarkDotNet.Running;
using QuerySpecification.Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

return;
var benchmark = new ExpressionBenchmark();

//var x1 = benchmark.EFIncludeExpression();
//var x2 = benchmark.EFIncludeString();
benchmark.Setup();
var x3 = benchmark.SpecIncludeExpression();
//var x4 = benchmark.SpecIncludeString();

//Console.WriteLine(x1);
//Console.WriteLine();
//Console.WriteLine(x2);
//Console.WriteLine();
//Console.WriteLine(x3);
//Console.WriteLine();
//Console.WriteLine(x4);
