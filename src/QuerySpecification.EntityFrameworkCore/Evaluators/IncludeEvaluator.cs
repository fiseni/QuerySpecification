using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Pozitron.QuerySpecification;

public sealed class IncludeEvaluator : IEvaluator
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

    private IncludeEvaluator() { }
    public static IncludeEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        foreach (var item in specification._state)
        {
            if (item is string includeString)
            {
                source = source.Include(includeString);
            }
        }

        bool isPreviousPropertyCollection = false;

        foreach (var item in specification._state)
        {
            if (item is IncludeThenExpression includeThenExpression)
            {
                source = BuildThenInclude<T>(source, includeThenExpression, isPreviousPropertyCollection);
                isPreviousPropertyCollection = IsCollection(includeThenExpression.LambdaExpression.ReturnType);
            }
            else if (item is IncludeExpression includeExpression)
            {
                source = BuildInclude<T>(source, includeExpression);
                isPreviousPropertyCollection = IsCollection(includeExpression.LambdaExpression.ReturnType);
            }
        }

        return source;
    }

    private static IQueryable<T> BuildInclude<T>(IQueryable source, IncludeExpression includeExpression)
    {
        Debug.Assert(includeExpression is not null);

        var result = _includeMethodInfo
            .MakeGenericMethod(typeof(T), includeExpression.LambdaExpression.ReturnType)
            .Invoke(null, [source, includeExpression.LambdaExpression]);

        Debug.Assert(result is not null);

        return (IQueryable<T>)result;
    }

    private static IQueryable<T> BuildThenInclude<T>(IQueryable source, IncludeThenExpression includeExpression, bool isPreviousPropertyCollection)
    {
        Debug.Assert(includeExpression is not null);

        var previousPropertyType = includeExpression.LambdaExpression.Parameters[0].Type;

        var result = (isPreviousPropertyCollection ? _thenIncludeAfterEnumerableMethodInfo : _thenIncludeAfterReferenceMethodInfo)
            .MakeGenericMethod(typeof(T), previousPropertyType, includeExpression.LambdaExpression.ReturnType)
            .Invoke(null, [source, includeExpression.LambdaExpression]);

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
