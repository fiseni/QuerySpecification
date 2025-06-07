namespace Pozitron.QuerySpecification;

/// <summary>
/// Specifies whether auto discovery for evaluators and validators is enabled.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class SpecAutoDiscoveryAttribute : Attribute
{
}

/// <summary>
/// Specifies discovery options for evaluators and validators, such as the order and whether discovery is enabled.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DiscoveryAttribute : Attribute
{
    /// <summary>
    /// Gets the order in which the evaluator/validator should be applied. Lower values are applied first.
    /// </summary>
    public int Order { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets a value indicating whether the evaluator/validator is discoverable.
    /// </summary>
    public bool Enable { get; set; } = true;
}

/// <summary>
/// Specifies discovery options for evaluators, such as the order and whether discovery is enabled.
/// </summary>
public sealed class EvaluatorDiscoveryAttribute : DiscoveryAttribute
{
}

/// <summary>
/// Specifies discovery options for validators, such as the order and whether discovery is enabled.
/// </summary>
public sealed class ValidatorDiscoveryAttribute : DiscoveryAttribute
{
}
