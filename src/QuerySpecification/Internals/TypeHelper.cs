using System.Reflection;

namespace Pozitron.QuerySpecification;

internal static class TypeHelper
{
    internal static readonly Lazy<Assembly[]> _loadedAssemblies = new(
        () => AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
                a.FullName != null &&
                !a.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
                !a.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) &&
                !a.FullName.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase) &&
                !a.FullName.StartsWith("Windows", StringComparison.OrdinalIgnoreCase)
            )
            .ToArray(),
        LazyThreadSafetyMode.ExecutionAndPublication);

    internal static readonly Lazy<Assembly[]> _specificationAssemblies = new(
        () => _loadedAssemblies.Value
            .Where(a => a.FullName!.StartsWith("Pozitron.QuerySpecification", StringComparison.OrdinalIgnoreCase))
            .ToArray(),
        LazyThreadSafetyMode.ExecutionAndPublication);

    internal static List<TType> GetInstancesOf<TType, TAttribute>(bool scanOnlySpecificationAssemblies)
        where TType : class
        where TAttribute : DiscoveryAttribute 
        => GetInstancesOf<TType, TAttribute>(scanOnlySpecificationAssemblies ? _specificationAssemblies.Value : _loadedAssemblies.Value);

    internal static List<TType> GetInstancesOf<TType, TAttribute>(IEnumerable<Assembly> assemblies)
        where TType : class
        where TAttribute : DiscoveryAttribute
    {
        var baseType = typeof(TType);
        var typeInstances = new List<(TType Instance, int Order, string TypeName)>();

        var types = assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); } catch { return Array.Empty<Type>(); }
            })
            .Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters && baseType.IsAssignableFrom(t))
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
                .Where(f => type.IsAssignableFrom(f.FieldType))
                .FirstOrDefault() is FieldInfo instanceField)
            {
                instance = (TType?)instanceField.GetValue(null);
            }
            else if (type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(f => type.IsAssignableFrom(f.PropertyType))
                .FirstOrDefault() is PropertyInfo instanceProp)
            {
                instance = (TType?)instanceProp.GetValue(null);
            }

            if (instance is null) continue;

            int order = discoveryAttr?.Order ?? int.MaxValue;
            typeInstances.Add((instance, order, type.Name));
        }

        return typeInstances
            .OrderBy(e => e.Order)
            .ThenBy(e => e.TypeName)
            .Select(e => e.Instance)
            .ToList();
    }
}
