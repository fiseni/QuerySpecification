using System.Buffers;
using System.Text.RegularExpressions;

namespace Pozitron.QuerySpecification;

/*
    Translates a SQL LIKE pattern into a matcher once, so per-row matching pays no repeated translation cost.
    The most common patterns (%term%, term%, %term, exact) contain no other wildcards, and are matched with plain
    string operations (vectorized, allocation-free, no regex). Anything else falls back to the regex translation.
    Matching is ordinal case-insensitive; the regex fallback uses CultureInvariant so both paths fold case the same way.
 */
internal abstract class LikeMatcher
{
    // Characters that require the regex fallback: wildcards '_' and '%', and '[' / '^' (character classes/negation).
    private static readonly SearchValues<char> _specialChars = SearchValues.Create("%_[^");

    public abstract bool IsMatch(string input);

    public static LikeMatcher Create(string pattern)
    {
        try
        {
            return CreateCore(pattern);
        }
        catch (Exception ex)
        {
            throw new InvalidLikePatternException(pattern, ex);
        }
    }

    private static LikeMatcher CreateCore(string pattern)
    {
        var body = pattern.AsSpan();

        var startsWithWildcard = body.Length > 0 && body[0] == '%';
        if (startsWithWildcard) body = body[1..];

        var endsWithWildcard = body.Length > 0 && body[^1] == '%';
        if (endsWithWildcard) body = body[..^1];

        if (body.ContainsAny(_specialChars))
        {
            return new RegexMatcher(BuildRegex(pattern));
        }

        var value = body.ToString();
        return (startsWithWildcard, endsWithWildcard) switch
        {
            (true, true) => new ContainsMatcher(value),
            (true, false) => new EndsWithMatcher(value),
            (false, true) => new StartsWithMatcher(value),
            (false, false) => new ExactMatcher(value),
        };
    }

    private static Regex BuildRegex(string pattern)
    {
        // Escape special regex characters, excluding those handled separately
        var regexPattern = Regex
            .Escape(pattern)
            .Replace("%", ".*")     // Translate SQL LIKE wildcard '%' to regex '.*'
            .Replace("_", ".")      // Translate SQL LIKE wildcard '_' to regex '.'
            .Replace(@"\[", "[")    // Unescape '[' as it's used for character classes/ranges
            .Replace(@"\^", "^");   // Unescape '^' as it can be used for negation in character classes

        // Ensure the pattern matches the entire string
        regexPattern = "^" + regexPattern + "$";

        // We intentionally don't use RegexOptions.Compiled. The patterns are dynamic (cached, but bounded and often
        // end-user supplied), and Compiled adds a ~1ms one-time build cost per pattern that is only recovered after
        // millions of matches of that same pattern. Interpreted is 9-980x faster for realistic match counts.
        // See Benchmark9_LikeRegexOptions.
        return new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private sealed class ExactMatcher(string value) : LikeMatcher
    {
        public override bool IsMatch(string input)
            => input.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class ContainsMatcher(string value) : LikeMatcher
    {
        public override bool IsMatch(string input)
            => input.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class StartsWithMatcher(string value) : LikeMatcher
    {
        public override bool IsMatch(string input)
            => input.StartsWith(value, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class EndsWithMatcher(string value) : LikeMatcher
    {
        public override bool IsMatch(string input)
            => input.EndsWith(value, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class RegexMatcher(Regex regex) : LikeMatcher
    {
        public override bool IsMatch(string input)
            => regex.IsMatch(input);
    }
}
