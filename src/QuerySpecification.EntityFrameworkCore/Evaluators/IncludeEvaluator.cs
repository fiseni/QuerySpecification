using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Collections.Concurrent;
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

    private static readonly ConcurrentDictionary<(Type Entity, Type ReturnType), MethodInfo> _methods = new();
    private static readonly ConcurrentDictionary<(Type Entity, Type PreviousProperty, Type ReturnType), MethodInfo> _methods2 = new();
    private static readonly ConcurrentDictionary<(Type Entity, Type PreviousProperty, Type ReturnType), MethodInfo> _methods3 = new();

    private static readonly ConcurrentDictionary<(Type EntityType, Type PropertyType, Type? PreviousPropertyType), Lazy<Func<IQueryable, LambdaExpression, IQueryable>>> _delegatesCache = new();

    private IncludeEvaluator() { }
    public static IncludeEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        if (specification.IsEmpty) return source;

        foreach (var state in specification._state)
        {
            if (state.Type == StateType.IncludeString && state.Reference is not null)
            {
                source = source.Include((string)state.Reference);
            }
        }

        bool isPreviousPropertyCollection = false;

        foreach (var state in specification._state)
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

        //Debug.Assert(includeExpression is not null);

        //var mi = _methods.GetOrAdd((typeof(T), includeExpression.ReturnType), static key =>
        //{
        //    Console.WriteLine("$$$$$$$$$$1");
        //    return _includeMethodInfo
        //        .MakeGenericMethod(key.Entity, key.ReturnType);
        //});

        //var result = mi.Invoke(null, [source, includeExpression]);

        //Debug.Assert(result is not null);

        //return (IQueryable<T>)result;

        //var include = _delegatesCache.GetOrAdd((typeof(T), includeExpression.ReturnType, null), CreateIncludeDelegate).Value;

        //return (IQueryable<T>)include(source, includeExpression);
    }


    private static Lazy<Func<IQueryable, LambdaExpression, IQueryable>> CreateIncludeDelegate((Type EntityType, Type PropertyType, Type? PreviousPropertyType) cacheKey)
        => new(() =>
    {
        var concreteInclude = _includeMethodInfo.MakeGenericMethod(cacheKey.EntityType, cacheKey.PropertyType);
        var sourceParameter = Expression.Parameter(typeof(IQueryable));
        var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

        var call = Expression.Call(
              concreteInclude,
              Expression.Convert(sourceParameter, typeof(IQueryable<>).MakeGenericType(cacheKey.EntityType)),
              Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PropertyType))));

        var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);

        return lambda.Compile();
    });

    private static IQueryable<T> BuildThenInclude<T>(IQueryable source, LambdaExpression includeExpression, bool isPreviousPropertyCollection)
    {
        Debug.Assert(includeExpression is not null);

        var previousPropertyType = includeExpression.Parameters[0].Type;

        var mi = isPreviousPropertyCollection
            ? _methods2.GetOrAdd((typeof(T), previousPropertyType, includeExpression.ReturnType), static key =>
            {
                Console.WriteLine("$$$$$$$$$$2");
                return _thenIncludeAfterEnumerableMethodInfo
                .MakeGenericMethod(key.Entity, key.PreviousProperty, key.ReturnType);
            })
            : _methods3.GetOrAdd((typeof(T), previousPropertyType, includeExpression.ReturnType), static key =>
            {
                Console.WriteLine("$$$$$$$$$$3");
                return _thenIncludeAfterReferenceMethodInfo
                .MakeGenericMethod(key.Entity, key.PreviousProperty, key.ReturnType);
            });

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
