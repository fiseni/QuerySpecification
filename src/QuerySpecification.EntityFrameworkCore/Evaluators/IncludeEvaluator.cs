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

        foreach (var state in specification.States)
        {
            if (state.Type == StateType.IncludeString && state.Reference is not null)
            {
                source = source.Include((string)state.Reference);
            }
        }

        bool isPreviousPropertyCollection = false;

        foreach (var state in specification.States)
        {
            if (state.Type == StateType.Include && state.Reference is not null)
            {
                var expr = (LambdaExpression)state.Reference;
                if (state.Bag == (int)IncludeTypeEnum.Include)
                {
                    source = BuildInclude<T>(source, expr);
                    isPreviousPropertyCollection = IsCollection(expr.ReturnType);
                }
                else if (state.Bag == (int)IncludeTypeEnum.ThenInclude)
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


    private static bool IsGenericEnumerable(Type type, out Type propertyType)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            propertyType = type.GenericTypeArguments[0];
            return true;
        }

        propertyType = type;
        return false;
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
