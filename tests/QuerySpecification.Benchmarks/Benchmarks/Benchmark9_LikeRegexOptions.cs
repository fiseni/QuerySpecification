using System.Text.RegularExpressions;

namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark9_LikeRegexOptions
{
    /* Informs whether RegexOptions.Compiled is worth it for the in-memory LIKE matching.
     * The in-memory evaluator builds a Regex once per unique pattern, then matches it once per source item.
     * So the relevant cost is: build the regex once + IsMatch it K times, where K is the number of rows.
     * RegexOptions.Compiled raises the build cost but lowers the per-match cost; the crossover depends on K.
     * This builds the regex on every invocation (the conservative case for Compiled, i.e. no cross-evaluation
     * cache reuse). When the pattern is reused across evaluations (the cache hits), Compiled is even more favorable.
     */

    private string _regexPattern = default!;
    private string[] _inputs = default!;

    [Params(1, 10, 100, 1000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // Same SQL-LIKE -> regex translation that LikeExtension uses, for the pattern "%tore%".
        _regexPattern = "^" + Regex.Escape("%tore%").Replace("%", ".*").Replace("_", ".") + "$";

        _inputs = new string[Rows];
        for (var i = 0; i < Rows; i++)
        {
            _inputs[i] = i % 2 == 0 ? $"Store {i}" : $"Company {i}";
        }
    }

    [Benchmark(Baseline = true)]
    public int Interpreted()
    {
        var regex = new Regex(_regexPattern, RegexOptions.IgnoreCase);
        return Count(regex, _inputs);
    }

    [Benchmark]
    public int Compiled()
    {
        var regex = new Regex(_regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        return Count(regex, _inputs);
    }

    private static int Count(Regex regex, string[] inputs)
    {
        var count = 0;
        foreach (var input in inputs)
        {
            if (regex.IsMatch(input)) count++;
        }
        return count;
    }
}
