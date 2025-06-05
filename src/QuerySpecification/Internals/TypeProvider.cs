namespace Pozitron.QuerySpecification;

internal class EvaluatorProvider
{
    public static List<IMemoryEvaluator> GetAllMemoryEvaluators() => _allMemoryEvaluators.Value.ToList();
    public static List<IMemoryEvaluator> GetBuiltInMemoryEvaluators() => _builtInMemoryEvaluators.Value.ToList();

    public static List<IEvaluator> GetAllEvaluators() => _allEvaluators.Value.ToList();
    public static List<IEvaluator> GetBuiltInEvaluators() => _builtInEvaluators.Value.ToList();


    private static readonly Lazy<List<IEvaluator>> _allEvaluators = new(
        () => TypeHelper.GetInstancesOf<IEvaluator, DiscoveryAttribute>(AppDomain.CurrentDomain.GetAssemblies()),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IEvaluator>> _builtInEvaluators = new(
        () => TypeHelper.GetInstancesOf<IEvaluator, DiscoveryAttribute>
            (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Pozitron.QuerySpecification"))),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IMemoryEvaluator>> _allMemoryEvaluators = new(
        () => TypeHelper.GetInstancesOf<IMemoryEvaluator, DiscoveryAttribute>(AppDomain.CurrentDomain.GetAssemblies()),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IMemoryEvaluator>> _builtInMemoryEvaluators = new(
        () => TypeHelper.GetInstancesOf<IMemoryEvaluator, DiscoveryAttribute>
            (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Pozitron.QuerySpecification"))),
        LazyThreadSafetyMode.ExecutionAndPublication);
}

internal class ValidatorProvider
{
    public static List<IValidator> GetAllValidators() => _allValidators.Value.ToList();
    public static List<IValidator> GetBuiltInValidators() => _builtInValidators.Value.ToList();


    private static readonly Lazy<List<IValidator>> _allValidators = new(
        () => TypeHelper.GetInstancesOf<IValidator, DiscoveryAttribute>(AppDomain.CurrentDomain.GetAssemblies()),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<List<IValidator>> _builtInValidators = new(
        () => TypeHelper.GetInstancesOf<IValidator, DiscoveryAttribute>
            (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName != null && x.FullName.StartsWith("Pozitron.QuerySpecification"))),
        LazyThreadSafetyMode.ExecutionAndPublication);
}
