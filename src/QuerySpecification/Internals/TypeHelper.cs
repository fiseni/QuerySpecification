using System.Reflection;

namespace Pozitron.QuerySpecification;

internal static class TypeHelper
{
    public static List<TType> GetInstancesOf<TType, TAttribute>(IEnumerable<Assembly> assemblies)
        where TType : class
        where TAttribute : DiscoveryAttribute
    {
        var evaluatorType = typeof(TType);
        var evaluators = new List<(TType Instance, int Order, string TypeName)>();

        var types = assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); } catch { return Array.Empty<Type>(); }
            })
            .Where(t => t.IsClass && !t.IsAbstract && evaluatorType.IsAssignableFrom(t))
            .Distinct();

        foreach (var type in types)
        {
            var discoveryAttr = type.GetCustomAttribute<TAttribute>();
            if (discoveryAttr is not null && !discoveryAttr.Enable)
                continue;

            TType? instance = null;

            if (type.GetConstructor(Type.EmptyTypes) is not null)
            {
                instance = (TType?)Activator.CreateInstance(type);
            }
            else if (type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => evaluatorType.IsAssignableFrom(f.FieldType))
                .FirstOrDefault() is FieldInfo instanceField)
            {
                instance = (TType?)instanceField.GetValue(null);
            }
            else if (type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(f => evaluatorType.IsAssignableFrom(f.PropertyType))
                .FirstOrDefault() is PropertyInfo instanceProp)
            {
                instance = (TType?)instanceProp.GetValue(null);
            }

            if (instance is null) continue;

            int order = discoveryAttr?.Order ?? int.MaxValue;
            evaluators.Add((instance, order, type.FullName ?? type.Name));
        }

        return evaluators
            .OrderBy(e => e.Order)
            .ThenBy(e => e.TypeName)
            .Select(e => e.Instance)
            .ToList();
    }
}
