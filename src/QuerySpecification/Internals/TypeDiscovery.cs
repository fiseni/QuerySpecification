using System.Reflection;

namespace Pozitron.QuerySpecification;

internal static class TypeDiscovery
{
    private static readonly Lazy<Assembly[]> _loadedAssemblies = new(
        () =>
        {
            try
            {
                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a =>
                        a.FullName != null &&
                        !a.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase) &&
                        !a.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase) &&
                        !a.FullName.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase) &&
                        !a.FullName.StartsWith("Windows", StringComparison.OrdinalIgnoreCase)
                    )
                    .ToArray();
            }
            catch
            {
                return [];
            }
        },
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<bool> _isAutoDiscoveryEnabled = new(
        () =>
        {
            try
            {
                return _loadedAssemblies
                    .Value
                    .Any(x => x.GetCustomAttributes().Any(attr => attr.GetType().Equals(typeof(SpecAutoDiscoveryAttribute))));
            }
            catch
            {
                return false;
            }
        },
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IEvaluator>> _evaluators = new(
        () => GetInstancesOf<IEvaluator, DiscoveryAttribute>(),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IMemoryEvaluator>> _memoryEvaluators = new(
        () => GetInstancesOf<IMemoryEvaluator, DiscoveryAttribute>(),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IValidator>> _validators = new(
        () => GetInstancesOf<IValidator, DiscoveryAttribute>(),
        LazyThreadSafetyMode.ExecutionAndPublication);


    internal static bool IsAutoDiscoveryEnabled => _isAutoDiscoveryEnabled.Value;
    internal static List<IMemoryEvaluator> GetMemoryEvaluators() => _memoryEvaluators.Value.ToList();
    internal static List<IEvaluator> GetEvaluators() => _evaluators.Value.ToList();
    internal static List<IValidator> GetValidators() => _validators.Value.ToList();

    internal static List<TType> GetInstancesOf<TType, TAttribute>()
        where TType : class
        where TAttribute : DiscoveryAttribute
        => GetInstancesOf<TType, TAttribute>(_loadedAssemblies.Value);

    internal static List<TType> GetInstancesOf<TType, TAttribute>(IEnumerable<Assembly> assemblies)
        where TType : class
        where TAttribute : DiscoveryAttribute
    {
        try
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
        catch
        {
            return [];
        }
    }
}
