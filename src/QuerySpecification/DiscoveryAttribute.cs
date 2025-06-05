namespace Pozitron.QuerySpecification;

/// <summary>
/// Specifies discovery options for evaluators and validators, such as order and whether discovery is enabled.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DiscoveryAttribute : Attribute
{
    /// <summary>
    /// Gets the order in which the evaluator should be applied. Lower values are applied first.
    /// </summary>
    public int Order { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets a value indicating whether the evaluator is discoverable.
    /// </summary>
    public bool Enable { get; set; } = true;
}

/// <summary>
/// Specifies discovery options for evaluators, such as order and whether discovery is enabled.
/// </summary>
public sealed class EvaluatorDiscoveryAttribute : DiscoveryAttribute
{
}

/// <summary>
/// Specifies discovery options for validators, such as order and whether discovery is enabled.
/// </summary>
public sealed class ValidatorDiscoveryAttribute : DiscoveryAttribute
{
}
