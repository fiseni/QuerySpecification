using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace QuerySpecification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark6_IncludeEvaluator
{
    /*
     * This benchmark only measures applying Include to IQueryable.
     * It tends to measure the pure overhead of the reflection calls.
     */

    private static readonly Expression<Func<Store, Company>> _includeCompany = x => x.Company;
    private static readonly Expression<Func<Company, Country>> _includeCountry = x => x.Country;

    private DbSet<Store> _queryable = default!;
    private Specification<Store> _spec = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
        _spec = new Specification<Store>();
        _spec.Query
            .Include(_includeCompany)
            .ThenInclude(_includeCountry);
    }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        var result = _queryable
            .Include(_includeCompany)
            .ThenInclude(_includeCountry);

        return result;
    }

    [Benchmark]
    public object Spec_MethodInvoke()
    {
        var evaluator = IncludeEvaluatorMethodInvoke.Instance;
        var result = evaluator.Evaluate(_queryable, _spec);

        return result;
    }

    [Benchmark]
    public object Spec_v11()
    {
        var evaluator = IncludeEvaluator.Instance;
        var result = evaluator.Evaluate(_queryable, _spec);
        return result;
    }

    private sealed class IncludeEvaluatorMethodInvoke : IEvaluator
    {
        private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 2
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

        private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                    && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                    && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                    && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

        private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                    && !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                    && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                    && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

        private IncludeEvaluatorMethodInvoke() { }
        public static IncludeEvaluatorMethodInvoke Instance = new();

        public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
        {
            if (specification.IsEmpty) return source;

            foreach (var state in specification.States)
            {
                if (state.Type == StateType.IncludeString && state.Reference is string includeString)
                {
                    source = source.Include(includeString);
                }
            }

            bool isPreviousPropertyCollection = false;

            foreach (var state in specification.States)
            {
                if (state.Type == StateType.Include && state.Reference is LambdaExpression expr)
                {
                    if (state.Bag == (int)IncludeType.Include)
                    {
                        source = BuildInclude<T>(source, expr);
                        isPreviousPropertyCollection = IsCollection(expr.ReturnType);
                    }
                    else if (state.Bag == (int)IncludeType.ThenInclude)
                    {
                        source = BuildThenInclude<T>(source, expr, isPreviousPropertyCollection);
                        isPreviousPropertyCollection = IsCollection(expr.ReturnType);
                    }
                }
            }

            return source;
        }

        private static IQueryable<T> BuildInclude<T>(IQueryable source, LambdaExpression includeExpression)

        {
            Debug.Assert(includeExpression is not null);

            var result = _includeMethodInfo
                .MakeGenericMethod(typeof(T), includeExpression.ReturnType)
                .Invoke(null, [source, includeExpression]);

            Debug.Assert(result is not null);

            return (IQueryable<T>)result;
        }


        private static IQueryable<T> BuildThenInclude<T>(IQueryable source, LambdaExpression includeExpression, bool isPreviousPropertyCollection)
        {
            Debug.Assert(includeExpression is not null);

            var previousPropertyType = includeExpression.Parameters[0].Type;

            var mi = isPreviousPropertyCollection
                ? _thenIncludeAfterEnumerableMethodInfo.MakeGenericMethod(typeof(T), previousPropertyType, includeExpression.ReturnType)
                : _thenIncludeAfterReferenceMethodInfo.MakeGenericMethod(typeof(T), previousPropertyType, includeExpression.ReturnType);

            var result = mi.Invoke(null, [source, includeExpression]);

            Debug.Assert(result is not null);

            return (IQueryable<T>)result;
        }

        public static bool IsCollection(Type type)
        {
            // Exclude string, which implements IEnumerable but is not considered a collection
            if (type == typeof(string))
            {
                return false;
            }

            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
