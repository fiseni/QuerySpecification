using System.Diagnostics;
using System.Reflection;

namespace Pozitron.QuerySpecification;

public class IncludeEvaluator : IEvaluator
{
    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
        .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
        .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 2);

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

    private IncludeEvaluator() { }
    public static IncludeEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        foreach (var includeString in specification.IncludeStrings)
        {
            source = source.Include(includeString);
        }

        foreach (var includeExpression in specification.IncludeExpressions)
        {
            if (includeExpression.Type == IncludeTypeEnum.Include)
            {
                source = BuildInclude<T>(source, includeExpression);
            }
            else if (includeExpression.Type == IncludeTypeEnum.ThenInclude)
            {
                source = BuildThenInclude<T>(source, includeExpression);
            }
        }

        return source;
    }

    private static IQueryable<T> BuildInclude<T>(IQueryable source, IncludeExpression includeExpression)
    {
        Debug.Assert(includeExpression is not null);

        var result = _includeMethodInfo
            .MakeGenericMethod(includeExpression.EntityType, includeExpression.PropertyType)
            .Invoke(null, [source, includeExpression.LambdaExpression]);

        Debug.Assert(result is not null);

        return (IQueryable<T>)result;
    }

    private static IQueryable<T> BuildThenInclude<T>(IQueryable source, IncludeExpression includeExpression)
    {
        Debug.Assert(includeExpression is not null);
        Debug.Assert(includeExpression.PreviousPropertyType is not null);

        var result = (IsGenericEnumerable(includeExpression.PreviousPropertyType, out var previousPropertyType)
                            ? _thenIncludeAfterEnumerableMethodInfo
                            : _thenIncludeAfterReferenceMethodInfo)
            .MakeGenericMethod(includeExpression.EntityType, previousPropertyType, includeExpression.PropertyType)
            .Invoke(null, [source, includeExpression.LambdaExpression]);

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
}
